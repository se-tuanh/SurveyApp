using SurveyApp.Models;

namespace SurveyApp.ViewModels
{
    public class HistoryViewModel
    {
        public int ResponseId { get; set; }
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public int TotalQuestions { get; set; }
        public bool IsActive { get; set; }
    }

    public class ResponseDetailViewModel
    {
        public int ResponseId { get; set; }
        public Survey Survey { get; set; } = null!;
        public List<Answer> UserAnswers { get; set; } = new();
        public DateTime SubmittedAt { get; set; }
    }
}
