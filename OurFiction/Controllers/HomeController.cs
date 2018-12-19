using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OurFiction.Data;
using OurFiction.Models;

namespace OurFiction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Repository repository;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            repository = new Repository(_context);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewData["ListOfActiveStories"] = repository.GetActiveStories();
            return View();
        }

        [AllowAnonymous]
        public IActionResult Read()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
