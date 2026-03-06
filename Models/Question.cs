using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Models
{
    public enum QuestionType
    {
        MultipleChoice,
        Text
    }

    public class Question
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nội dung câu hỏi là bắt buộc")]
        [Display(Name = "Nội dung câu hỏi")]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Loại câu hỏi")]
        public QuestionType QuestionType { get; set; } = QuestionType.MultipleChoice;

        public int Order { get; set; } = 0;

        // FK
        public int SurveyId { get; set; }

        // Navigation
        public Survey? Survey { get; set; }
        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
