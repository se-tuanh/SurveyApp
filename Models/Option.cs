using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Models
{
    public class Option
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nội dung lựa chọn là bắt buộc")]
        [Display(Name = "Lựa chọn")]
        public string Text { get; set; } = string.Empty;

        // FK
        public int QuestionId { get; set; }

        // Navigation
        public Question? Question { get; set; }
    }
}
