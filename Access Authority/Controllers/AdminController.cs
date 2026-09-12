using Access_Authority.Data;
using Access_Authority.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Access_Authority.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _Context;
        private readonly IWebHostEnvironment _env;
        public AdminController(AppDbContext context, IWebHostEnvironment env)
        {
            _Context = context;
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel();

            // STATISTICS

            model.TotalBlogs = await _Context.CreateBlogs.CountAsync();

            model.TotalContacts = await _Context.ContactViewModels.CountAsync();

            model.OpenPositions = await _Context.CareerPositions
                .CountAsync(x => x.Status == "Open");

            // LATEST BLOGS

            model.LatestBlogs = await _Context.CreateBlogs
                .OrderByDescending(x => x.CreatedDate)
                .Take(3)
                .ToListAsync();

            // LATEST CONTACTS

            model.LatestContacts = await _Context.ContactViewModels
                .OrderByDescending(x => x.CreatedAt)
                .Take(2)
                .ToListAsync();

            // LATEST CAREER POSITIONS

            model.LatestPositions = await _Context.CareerPositions
                .OrderByDescending(x => x.PostedDate)
                .Take(3)
                .ToListAsync();

            // LATEST ACTIVITY

            var activities = new List<DashboardActivity>();

            var blogs = await _Context.CreateBlogs
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToListAsync();

            foreach (var blog in blogs)
            {
                activities.Add(new DashboardActivity
                {
                    Title = $"Blog post — \"{blog.Title}\"",
                    Type = "Blog",
                    Status = blog.Status,
                    Date = blog.CreatedDate
                });
            }


            var contacts = await _Context.ContactViewModels
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .ToListAsync();

            foreach (var contact in contacts)
            {
                activities.Add(new DashboardActivity
                {
                    Title = $"Message from {contact.FullName}",
                    Type = "Contact",
                    Status = "New",
                    Date = contact.CreatedAt
                });
            }


            var positions = await _Context.CareerPositions
                .OrderByDescending(x => x.PostedDate)
                .Take(5)
                .ToListAsync();

            foreach (var position in positions)
            {
                activities.Add(new DashboardActivity
                {
                    Title = $"Opening — {position.JobTitle}",
                    Type = "Career",
                    Status = position.Status,
                    Date = position.PostedDate
                });
            }


            model.LatestActivities = activities
                .OrderByDescending(x => x.Date)
                .Take(6)
                .ToList();


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Career(int page = 1, int applicantPage = 1, int? positionId = null)
        {
            int pageSize = 3;
            int applicantPageSize = 5;

            // =========================
            // POSITIONS
            // =========================

            var totalPositions = await _Context.CareerPositions.CountAsync();

            var positions = await _Context.CareerPositions
                .OrderByDescending(x => x.PostedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Dropdown ke liye ALL positions
            var allPositions = await _Context.CareerPositions
                .OrderBy(x => x.JobTitle)
                .ToListAsync();


            // =========================
            // APPLICANTS
            // =========================

            var applicantQuery = _Context.CareerViewModels
                .Include(x => x.CareerPosition)
                .AsQueryable();

            // Position filter
            if (positionId.HasValue)
            {
                applicantQuery = applicantQuery
                    .Where(x => x.CareerPositionId == positionId.Value);
            }

            var totalApplicants = await applicantQuery.CountAsync();
            ViewBag.TotalApplicants = totalApplicants;

            var applications = await applicantQuery
                .OrderByDescending(x => x.AppliedAt)
                .Skip((applicantPage - 1) * applicantPageSize)
                .Take(applicantPageSize)
                .ToListAsync();


            // =========================
            // VIEWBAG
            // =========================

            ViewBag.CareerApplications = applications;

            ViewBag.AllCareerPositions = allPositions;

            ViewBag.SelectedPositionId = positionId;

            ViewBag.CurrentPage = page;

            ViewBag.TotalPages = (int)Math.Ceiling(
                totalPositions / (double)pageSize
            );

            ViewBag.TotalPositions = totalPositions;

            ViewBag.ApplicantCurrentPage = applicantPage;

            ViewBag.ApplicantTotalPages = Math.Max(
                1,
                (int)Math.Ceiling(
                    totalApplicants / (double)applicantPageSize
                )
            );

            ViewBag.TotalApplicants = totalApplicants;

            return View(positions);
        }

        // VIEW APPLICATION
        [HttpGet]
        public async Task<IActionResult> ViewApplication(int id)
        {
            var application = await _Context.CareerViewModels
                .Include(x => x.CareerPosition)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            return Json(new
            {
                fullName = application.FullName,
                emailAddress = application.EmailAddress,
                phoneNumber = application.PhoneNumber,
                linkedInProfile = application.LinkedInProfile,
                resumeFilePath = application.ResumeFilePath,
                appliedAt = application.AppliedAt,
                position = application.CareerPosition != null
                    ? application.CareerPosition.JobTitle
                    : "N/A"
            });
        }
        // DELETE APPLICATION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _Context.CareerViewModels
                .FirstOrDefaultAsync(x => x.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            _Context.CareerViewModels.Remove(application);

            await _Context.SaveChangesAsync();

            TempData["Success"] = "Application deleted successfully.";

            return RedirectToAction(nameof(Career));
        }
        //================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPosition(CareerPosition model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return RedirectToAction(nameof(Career));
            }

            model.Status = "Open";
            model.PostedDate = DateTime.Now;

            _Context.CareerPositions.Add(model);
            await _Context.SaveChangesAsync();

            TempData["Success"] = "Position published successfully!";

            return RedirectToAction(nameof(Career));
        }
        [HttpGet]
        public async Task<IActionResult> EditPosition(int id)
        {
            var position = await _Context.CareerPositions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (position == null)
            {
                TempData["Error"] = "Position not found.";
                return RedirectToAction(nameof(Career));
            }

            return View(position);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPosition(CareerPosition model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return RedirectToAction(nameof(Career));
            }

            var position = await _Context.CareerPositions
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (position == null)
            {
                TempData["Error"] = "Position not found.";
                return RedirectToAction(nameof(Career));
            }

            position.JobTitle = model.JobTitle;
            position.Description = model.Description;
            position.Location = model.Location;
            position.Type = model.Type;
            position.Status = model.Status;

            await _Context.SaveChangesAsync();

            TempData["Success"] = "Position updated successfully!";

            return RedirectToAction(nameof(Career));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _Context.CareerPositions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (position == null)
            {
                return NotFound();
            }

            _Context.CareerPositions.Remove(position);

            await _Context.SaveChangesAsync();

            return RedirectToAction("Career");
        }
        [HttpGet]
        public async Task<IActionResult> Blog(int page = 1)
        {
            int pageSize = 5;

            // Database ke ALL blogs ka total
            int totalPosts = await _Context.CreateBlogs.CountAsync();

            // Current page ke blogs
            var blogs = await _Context.CreateBlogs
                .OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.TotalPosts = totalPosts;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            return View(blogs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _Context.CreateBlogs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            _Context.CreateBlogs.Remove(blog);

            await _Context.SaveChangesAsync();

            TempData["Success"] = "Blog deleted successfully!";

            return RedirectToAction(nameof(Blog));
        }
        [HttpGet]
        public async Task<IActionResult> ViewBlog(int id)
        {
            var blog = await _Context.CreateBlogs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }
        [HttpGet]
        public IActionResult CreateBlog()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlog(CreateBlog blog, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }

            if (image != null && image.Length > 0)
            {
                string uploadFolder = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "blog"
                );

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(image.FileName);

                string filePath = Path.Combine(
                    uploadFolder,
                    fileName
                );

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                blog.FeaturedImage = "/uploads/blog/" + fileName;
            }

            blog.CreatedDate = DateTime.UtcNow;

            _Context.CreateBlogs.Add(blog);

            await _Context.SaveChangesAsync();

            TempData["Success"] = "Blog published successfully!";

            return RedirectToAction(nameof(Blog));
        }
        public async Task<IActionResult> Contact()
        {
            var contacts = await _Context.ContactViewModels
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            return View(contacts);
        }
        [HttpGet]
        public async Task<IActionResult> ViewContact(int id)
        {
            var contact = await _Context.ContactViewModels
                .FirstOrDefaultAsync(x => x.Id == id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _Context.ContactViewModels.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            _Context.ContactViewModels.Remove(contact);

            await _Context.SaveChangesAsync();

            return RedirectToAction("Contact");
        }
        [HttpGet]
        public IActionResult CreateProject()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(Project model)
        {
           

            ModelState.Remove(nameof(Project.HeroImage));

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "projects"
                );

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string extension = Path.GetExtension(model.ImageFile.FileName);

                string uniqueFileName =
                    Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName
                );

                using (var fileStream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }

                model.HeroImage = "/uploads/projects/" + uniqueFileName;
            }

            // ==========================================
            // IMAGE REQUIRED
            // ==========================================

            if (string.IsNullOrWhiteSpace(model.HeroImage))
            {
                ModelState.AddModelError(
                    "ImageFile",
                    "Please select a project image."
                );
            }

            // ==========================================
            // AUTO SLUG
            // ==========================================

            if (string.IsNullOrWhiteSpace(model.Slug))
            {
                model.Slug = model.Title
                    .ToLower()
                    .Trim()
                    .Replace(" ", "-");
            }

            // ==========================================
            // FINAL MODEL VALIDATION
            // ==========================================

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e =>
                        $"{x.Key}: {e.ErrorMessage}"
                    ))
                    .ToList();

                TempData["Error"] = string.Join(" | ", errors);

                return View(model);
            }

            model.CreatedDate = DateTime.Now;

            _Context.Projects.Add(model);

            await _Context.SaveChangesAsync();

         
            TempData["Success"] = "Project created successfully!";

            return RedirectToAction("Projects");
        }
        public async Task<IActionResult> Projects(int page = 1)
        {
            int pageSize = 5;

            if (page < 1)
            {
                page = 1;
            }

            int totalProjects = await _Context.Projects.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalProjects / (double)pageSize
            );

            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var projects = await _Context.Projects
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalProjects = totalProjects;

            return View(projects);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _Context.Projects
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                TempData["Error"] = "Project not found!";
                return RedirectToAction(nameof(Projects));
            }

            // Delete project image
            if (!string.IsNullOrWhiteSpace(project.HeroImage))
            {
                var imagePath = Path.Combine(
                    _env.WebRootPath,
                    project.HeroImage
                        .TrimStart('/')
                        .Replace("/", Path.DirectorySeparatorChar.ToString())
                );

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _Context.Projects.Remove(project);

            await _Context.SaveChangesAsync();

            TempData["Success"] = "Project deleted successfully!";

            return RedirectToAction(nameof(Projects));
        }
    }
}