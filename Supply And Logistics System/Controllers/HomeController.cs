using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Supply_And_Logistics_System.Models;

namespace Supply_And_Logistics_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Endpoint de teste para verificar se o sistema está a funcionar
        [HttpGet]
        public IActionResult Test()
        {
            return Content("System is running!");
        }

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
