using AptechVision.Data;
using AptechVision.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AptechVision.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Feedback/Submit
        [HttpPost]
        public IActionResult Submit(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.SubmittedAt = DateTime.Now;
                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                return RedirectToAction("About", "Home");
            }
            return View(feedback);
        }

        // GET: Feedback/List
        public IActionResult List()
        {
            var feedbacks = _context.Feedbacks.OrderByDescending(f => f.SubmittedAt).ToList();
            return PartialView("_FeedbackList", feedbacks);
        }
    }
}
