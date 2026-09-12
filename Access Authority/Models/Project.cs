using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Authority.Models
{
    public class Project
    {
        public int Id { get; set; }

        // PROJECT CARD DETAILS

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = "";

        [Required]
        [StringLength(500)]
        public string ShortDescription { get; set; } = "";

        [Required]
        public string HeroImage { get; set; } = "";

        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = "";

        // PROJECT BASIC INFORMATION

        [StringLength(200)]
        public string Client { get; set; } = "";

        [StringLength(100)]
        public string Industry { get; set; } = "";

        [StringLength(100)]
        public string Duration { get; set; } = "";

        [StringLength(100)]
        public string Team { get; set; } = "";

        // CASE STUDY CONTENT

        public string Overview { get; set; } = "";

        public string Challenge { get; set; } = "";

        public string Solution { get; set; } = "";

        public string Result { get; set; } = "";

        [StringLength(100)]
        public string Metric1Value { get; set; } = "";

        [StringLength(150)]
        public string Metric1Label { get; set; } = "";

        [StringLength(100)]
        public string Metric1Icon { get; set; } = "";

        [StringLength(100)]
        public string Metric2Value { get; set; } = "";

        [StringLength(150)]
        public string Metric2Label { get; set; } = "";

        [StringLength(100)]
        public string Metric2Icon { get; set; } = "";

        public string Technologies { get; set; } = "";

        public string Features { get; set; } = "";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}