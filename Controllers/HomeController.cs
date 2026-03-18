using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApp.Data;

namespace SurveyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string filter = "all")
        {
            var query = _context.Surveys
                .Include(s => s.CreatedBy)
                .Include(s => s.Questions)
                .Include(s => s.Responses)
                .AsQueryable();

            if (filter == "open")
            {

                query = query.Where(s => s.IsActive && (!s.ClosedAt.HasValue || s.ClosedAt.Value > DateTime.Now));
            }
            else if (filter == "closed")
            {

                query = query.Where(s => !s.IsActive || (s.ClosedAt.HasValue && s.ClosedAt.Value <= DateTime.Now));
            }
            else 
            {

                query = query.Where(s => s.IsActive);
            }

            var surveys = await query
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            ViewBag.CurrentFilter = filter;
            return View(surveys);
        }
    }
}
