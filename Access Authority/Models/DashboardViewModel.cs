using System;
using System.Collections.Generic;

namespace Access_Authority.Models
{
    public class DashboardViewModel
    {
        // Statistics
        public int TotalBlogs { get; set; }
        public int TotalContacts { get; set; }
        public int OpenPositions { get; set; }

        // Latest data
        public List<CreateBlog> LatestBlogs { get; set; } = new();
        public List<ContactViewModel> LatestContacts { get; set; } = new();
        public List<CareerPosition> LatestPositions { get; set; } = new();

        // Combined activity
        public List<DashboardActivity> LatestActivities { get; set; } = new();
    }

    public class DashboardActivity
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}