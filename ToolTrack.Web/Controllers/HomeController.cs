using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToolTrack.Infrastructure.Persistence;
using ToolTrack.Web.Models;

namespace ToolTrack.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ToolTrackDbContext _context;

        public HomeController(ILogger<HomeController> logger, ToolTrackDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Total herramientas registradas
            ViewBag.TotalHerramientas = _context.Herramientas.Count();

            // Herramientas prestadas (Estado = Prestada)
            ViewBag.TotalPrestadas = _context.Herramientas.Count(h => h.Estado == "Prestada");

            // Herramientas disponibles (Estado = Disponible)
            ViewBag.TotalDisponibles = _context.Herramientas.Count(h => h.Estado == "Disponible");

            // Alerta de devolución: prestamos activos con fecha de devolución vencida
            ViewBag.AlertaDevolucion = _context.Prestamos
                .Count(p => p.Estado == "Activo" && p.FechaDevolucion != null && p.FechaDevolucion < DateTime.Now);

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
