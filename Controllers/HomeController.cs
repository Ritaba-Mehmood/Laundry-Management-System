using AptechVision.Data;
using AptechVision.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using aptechvision.Models;

namespace AptechVision.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var feedbacks = _context.Feedbacks.OrderByDescending(f => f.SubmittedAt).ToList();
            ViewBag.Feedbacks = feedbacks;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> About(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.SubmittedAt = DateTime.Now;
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
                return RedirectToAction("About");
            }

            var feedbacks = _context.Feedbacks.OrderByDescending(f => f.SubmittedAt).ToList();
            ViewBag.Feedbacks = feedbacks;
            return View(feedback);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> Pricing()
        {
            var packages = await _context.Packages.ToListAsync();
            return View(packages);
        }

        // GET: OrderNow
        [HttpGet]
        public IActionResult OrderNow(int packageId)
        {
            var package = _context.Packages.Find(packageId);
            if (package == null)
            {
                return NotFound();
            }
            var viewModel = new OrderViewModel
            {
                Package = package,
                PackageId = packageId
            };
            return View(viewModel);
        }

        // POST: PlaceOrder
        [HttpPost]
        public async Task<IActionResult> OrderNow(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    Name = model.Name,
                    Phone = model.Phone,
                    PickupDate = model.PickupDate,
                    Slot = model.Slot,
                    Email = model.Email,
                    Address = model.Address,
                    PackageId = model.PackageId
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Redirect to OrderConfirmation with the correct parameter name 'id'
                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }
            return View(model);
        }

        public IActionResult OrderConfirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.Package)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }



    }
}
