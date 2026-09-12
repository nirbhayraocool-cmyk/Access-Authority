namespace Access_Authority.Models
{
    public class ProjectDetailsViewModel
    {
        public string Slug { get; set; } = "";
        public string Category { get; set; } = "";
        public string Title { get; set; } = "";
        public string ShortDescription { get; set; } = "";
        public string HeroImage { get; set; } = "";

        public string Client { get; set; } = "";
        public string Industry { get; set; } = "";
        public string Duration { get; set; } = "";
        public string Team { get; set; } = "";

        public string Overview { get; set; } = "";
        public string Challenge { get; set; } = "";
        public string Solution { get; set; } = "";
        public string Result { get; set; } = "";
        public List<string> Technologies { get; set; } = new();
        public List<string> Features { get; set; } = new();
        public List<ProjectMetric> Metrics { get; set; } = new();
    }

    public class ProjectMetric
    {
        public string Value { get; set; } = "";
        public string Label { get; set; } = "";
    }
}