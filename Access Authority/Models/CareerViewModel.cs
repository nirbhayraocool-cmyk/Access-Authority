using System.ComponentModel.DataAnnotations.Schema;

namespace Access_Authority.Models
{
    public class CareerViewModel
    {
        public int Id { get; set; }

        public int? CareerPositionId { get; set; }

        public CareerPosition? CareerPosition { get; set; }

        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string LinkedInProfile { get; set; }

        [NotMapped]
        public IFormFile? Resume { get; set; }

        public string ResumeFileName { get; set; }
        public string ResumeFilePath { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
