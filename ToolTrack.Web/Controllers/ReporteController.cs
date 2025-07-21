using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolTrack.Infrastructure.Persistence;

namespace ToolTrack.Web.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ToolTrackDbContext _context;

        public ReportesController(ToolTrackDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalPrestamos = await _context.Prestamos.CountAsync();
            var totalDevueltos = await _context.Prestamos.CountAsync(p => p.Estado == "Devuelto");
            var totalActivos = await _context.Prestamos.CountAsync(p => p.Estado == "Activo");

            var herramientaMasPrestada = await _context.Prestamos
                .GroupBy(p => p.IdHerramienta)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    Herramienta = g.First().Herramienta.Nombre,
                    Cantidad = g.Count()
                })
                .FirstOrDefaultAsync();

            var trabajadorTop = await _context.Prestamos
                .GroupBy(p => p.IdTrabajador)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    Trabajador = g.First().Trabajador.Nombres,
                    Cantidad = g.Count()
                })
                .FirstOrDefaultAsync();

            ViewBag.TotalPrestamos = totalPrestamos;
            ViewBag.TotalDevueltos = totalDevueltos;
            ViewBag.TotalActivos = totalActivos;
            ViewBag.HerramientaMasPrestada = herramientaMasPrestada;
            ViewBag.TrabajadorTop = trabajadorTop;

            return View();
        }
    }
}
