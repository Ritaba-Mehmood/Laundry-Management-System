using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AptechVision.Data;
using AptechVision.Models;
using aptechvision.Models;


namespace AptechVision.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard view
        public IActionResult Dashboard()
        {
            return View();
        }

        // Display all orders
 
        // Display details for an individual order
     }
}
