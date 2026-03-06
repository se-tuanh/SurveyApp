namespace SurveyApp.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalSurveys { get; set; }
        public int ActiveSurveys { get; set; }
        public int TotalResponses { get; set; }
        public int TotalUsers { get; set; }
        public List<SurveyStat> TopSurveys { get; set; } = new();
        public List<RecentResponse> RecentResponses { get; set; } = new();
    }

    public class SurveyStat
    {
        public int SurveyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ResponseCount { get; set; }
        public int QuestionCount { get; set; }
        public bool IsActive { get; set; }
    }

    public class RecentResponse
    {
        public string SurveyTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
    }

    public class UserManagementViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<string> Roles { get; set; } = new();
        public int SurveyCount { get; set; }
        public int ResponseCount { get; set; }
    }
}
