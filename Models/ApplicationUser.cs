using Microsoft.AspNetCore.Identity;

namespace SurveyApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Survey> Surveys { get; set; } = new List<Survey>();
        public ICollection<Response> Responses { get; set; } = new List<Response>();
    }
}
