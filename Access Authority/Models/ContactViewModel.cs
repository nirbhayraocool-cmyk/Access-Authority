using System.ComponentModel.DataAnnotations;

namespace Access_Authority.Models
{
    public class ContactViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? ServiceInterest { get; set; }

        [Required]
        public string ProjectDetails { get; set; }

        public bool PrivacyAccepted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
