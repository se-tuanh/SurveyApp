using System.ComponentModel.DataAnnotations;

namespace SurveyApp.Models
{
    public class Survey
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không quá 200 ký tự")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; } = true;

        // FK
        public string CreatedById { get; set; } = string.Empty;

        // Navigation
        public ApplicationUser? CreatedBy { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Response> Responses { get; set; } = new List<Response>();
    }
}
