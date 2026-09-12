using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Authority.Models
{
    public class CareerViewApply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [StringLength(200)]
        [Url]
        public string? LinkedInProfile { get; set; }  

        [Required]
        [StringLength(500)]
        public string ResumePath { get; set; }    
        
        [NotMapped]
        public IFormFile? Resume { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
    }
}
