using System.ComponentModel.DataAnnotations;

namespace Access_Authority.Models
{
    public class CreateBlog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; }

        [Required]
        public string Status { get; set; } = "Draft";

        [Required]
        public string Content { get; set; }

        [StringLength(500)]
        public string? FeaturedImage { get; set; }

        [StringLength(500)]
        public string? Tags { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }
}
