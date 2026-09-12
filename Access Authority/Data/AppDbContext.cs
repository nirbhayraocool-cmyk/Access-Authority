using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Access_Authority.Models;
using Microsoft.EntityFrameworkCore;
namespace Access_Authority.Data
{
    public class AppDbContext:IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
       public DbSet<ContactViewModel> ContactViewModels { get; set; }
       public DbSet<CareerViewModel> CareerViewModels { get; set; }

       public DbSet<CareerViewApply> CareerViewApplys { get; set; }

       public DbSet<CareerPosition> CareerPositions { get; set; }
       public DbSet<CreateBlog> CreateBlogs { get; set; }
       public DbSet<Project> Projects { get; set; }
    }

}
