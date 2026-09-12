using System.ComponentModel.DataAnnotations;

namespace Access_Authority.Models
{
    public class CareerPosition
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string JobTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [StringLength(50)]
        public string? Type { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Open";

        public DateTime PostedDate { get; set; } = DateTime.Now;
    }
}
