using GestionImpresoras.Data;
using GestionImpresoras.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GestionImpresoras.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext _contexto;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDBContext contexto, ILogger<HomeController> logger)
        {
            this._contexto = contexto;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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