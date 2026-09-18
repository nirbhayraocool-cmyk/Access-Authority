using Access_Authority.Data;
using Access_Authority.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Access_Authority.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly AppDbContext _Context;
        public HomeController(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Context = context;
        }

        // LOGIN GET
       
        [HttpGet]
        public IActionResult LogIn()
        {
            GenerateCaptcha();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                GenerateCaptcha();
                return View(model);
            }

            var captchaCode =
                HttpContext.Session.GetString("CaptchaCode");

            if (string.IsNullOrEmpty(captchaCode) ||
                !string.Equals(
                    captchaCode,
                    model.Captcha,
                    StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    "Captcha",
                    "Invalid CAPTCHA");

                GenerateCaptcha();

                return View(model);
            }


            var user = await _userManager
                .FindByNameAsync(model.UserName);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Username or Password incorrect");

                GenerateCaptcha();

                return View(model);
            }


          
            // PASSWORD CHECK
            
            var result =
                await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    false,
                    false);


            
            // LOGIN SUCCESS
           
            if (result.Succeeded)
            {
                return RedirectToAction(
                    "Index",
                    "Admin");
            }


            
            // WRONG PASSWORD
            
            ModelState.AddModelError(
                "",
                "Username or Password incorrect");

            GenerateCaptcha();

            return View(model);
        }


        // ==============================
        // CAPTCHA REFRESH
        // ==============================
        [HttpGet]
        public IActionResult RefreshCaptcha()
        {
            GenerateCaptcha();

            return RedirectToAction("LogIn");
        }


        // ==============================
        // CAPTCHA METHOD
        // ==============================
        private void GenerateCaptcha()
        {
            const string chars =
                "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            Random random = new Random();

            string captcha = "";

            for (int i = 0; i < 6; i++)
            {
                captcha += chars[random.Next(chars.Length)];
            }

            HttpContext.Session.SetString(
                "CaptchaCode",
                captcha);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }
        // OTHER HOME ACTIONS

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogs = await _Context.CreateBlogs
                .Where(x => x.Status.ToLower() == "published")
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync(); 
            return View(blogs);
        }

        public IActionResult Product()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Blog() 
        { 
            var blogs = await _Context.CreateBlogs
                .Where(x => x.Status.ToLower() == "published")
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync(); return View(blogs);
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _Context.ContactViewModels.Add(model);
            await _Context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Your message has been sent successfully!"
            });
        }
        public IActionResult Policy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Portfolio()
        {
            var projects = await _Context.Projects
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(projects);
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult WebDevlopment()
        {
            return View();
        }
        public IActionResult ERPDevlopment()
        {
            return View();
        }
        public IActionResult MobileApps()
        {
            return View();
        }
        public IActionResult UIUXDisign()
        {
            return View();
        }
        public IActionResult VideoProduction()
        {
            return View();
        }
        public IActionResult GraphicDesign()
        {
            return View();
        }
        public async Task<IActionResult> Projects()
        {
            var projects = await _Context.Projects
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(projects);
        }
        public IActionResult FreeConsultation()
        {
            return View();
        }
        public IActionResult CustomSoftware()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Career()
        {
            var positions = await _Context.CareerPositions
                .Where(x => x.Status == "Open")
                .OrderByDescending(x => x.PostedDate)
                .ToListAsync();

            ViewBag.CareerPositions = positions;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Career(CareerViewModel model)
        {
             var positions = await _Context.CareerPositions
             .Where(x => x.Status == "Open")
             .OrderByDescending(x => x.PostedDate)
             .ToListAsync();

            ViewBag.CareerPositions = positions;

            ModelState.Remove(nameof(CareerViewModel.ResumeFileName));
            ModelState.Remove(nameof(CareerViewModel.ResumeFilePath));
            ModelState.Remove(nameof(CareerViewModel.AppliedAt));

            // Resume required
            if (model.Resume == null || model.Resume.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(CareerViewModel.Resume),
                    "Please upload your resume."
                );
            }

            // Validation errors
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e =>
                        $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();

                TempData["Error"] = string.Join(" | ", errors);

                return View(model);
            }

            // Maximum 5 MB
            if (model.Resume!.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] = "Resume size must be less than 5 MB.";

                return View(model);
            }

            // Allowed extensions
            string extension = Path
                .GetExtension(model.Resume.FileName)
                .ToLowerInvariant();

            string[] allowedExtensions =
            {
        ".pdf",
        ".doc",
        ".docx"
    };

            if (!allowedExtensions.Contains(extension))
            {
                TempData["Error"] =
                    "Only PDF, DOC and DOCX files are allowed.";

                return View(model);
            }

            // Upload folder
            string uploadFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "resumes"
            );

            Directory.CreateDirectory(uploadFolder);

            // Unique file name
            string uniqueFileName =
                Guid.NewGuid().ToString() + extension;

            string physicalPath =
                Path.Combine(uploadFolder, uniqueFileName);

            try
            {
                // Save resume
                using (var stream = new FileStream(
                    physicalPath,
                    FileMode.Create))
                {
                    await model.Resume.CopyToAsync(stream);
                }

                // Create database record
                var career = new CareerViewModel
                {
                    FullName = model.FullName,
                    EmailAddress = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                    LinkedInProfile = model.LinkedInProfile,

                    CareerPositionId = model.CareerPositionId,

                    ResumeFileName = model.Resume.FileName,

                    ResumeFilePath =
          "/uploads/resumes/" + uniqueFileName,

                    AppliedAt = DateTime.Now
                };

                _Context.CareerViewModels.Add(career);

                await _Context.SaveChangesAsync();

                TempData["Success"] =
                    "Career application submitted successfully.";

                return RedirectToAction(nameof(Career));
            }
            catch (Exception ex)
            {
                // DB save fail hone par uploaded file delete
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }

                TempData["Error"] =
                    "Application could not be saved: " + ex.Message;

                return View(model);
            }
        }
        public IActionResult Team()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Blogdetail(int id)
        {
            var blog = await _Context.CreateBlogs
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.Status.ToLower() == "published");

            if (blog == null)
            {
                return NotFound();
            }

            var recentPosts = await _Context.CreateBlogs
                .Where(x =>
                    x.Status.ToLower() == "published" &&
                    x.Id != id)
                .OrderByDescending(x => x.CreatedDate)
                .Take(2)
                .ToListAsync();

            ViewBag.RecentPosts = recentPosts;

            return View(blog);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ProjectDetail(int id)
        {
            var project = await _Context.Projects
                .FirstOrDefaultAsync(x => x.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
    }
}