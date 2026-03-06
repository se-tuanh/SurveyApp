namespace SurveyApp.Models
{
    public class Answer
    {
        public int Id { get; set; }

        // For text-type questions
        public string? TextAnswer { get; set; }

        // FK
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        public int? SelectedOptionId { get; set; }

        // Navigation
        public Response? Response { get; set; }
        public Question? Question { get; set; }
        public Option? SelectedOption { get; set; }
    }
}
