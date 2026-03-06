namespace SurveyApp.Models
{
    public class Response
    {
        public int Id { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // FK
        public int SurveyId { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Navigation
        public Survey? Survey { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
