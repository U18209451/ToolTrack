using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToolTrack.Domain.Entities;
using ToolTrack.Infrastructure.Persistence;

namespace ToolTrack.Web.Controllers
{
    public class PrestamoesController : Controller
    {
        private readonly ToolTrackDbContext _context;

        public PrestamoesController(ToolTrackDbContext context)
        {
            _context = context;
        }

        // GET: Prestamoes (solo activos)
        public async Task<IActionResult> Index()
        {
            var prestamosActivos = await _context.Prestamos
                .Include(p => p.Herramienta)
                .Include(p => p.Trabajador)
                .Where(p => p.Estado == "Activo")
                .ToListAsync();

            return View(prestamosActivos);
        }

        // GET: Prestamoes/Devoluciones (solo devueltos)
        public async Task<IActionResult> Devoluciones()
        {
            var prestamosDevueltos = await _context.Prestamos
                .Include(p => p.Herramienta)
                .Include(p => p.Trabajador)
                .Where(p => p.Estado == "Devuelto")
                .ToListAsync();

            return View(prestamosDevueltos);
        }

        // GET: Prestamoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var prestamo = await _context.Prestamos
                .Include(p => p.Herramienta)
                .Include(p => p.Trabajador)
                .FirstOrDefaultAsync(m => m.IdPrestamo == id);

            if (prestamo == null)
                return NotFound();

            return View(prestamo);
        }

        // GET: Prestamoes/Create
        public IActionResult Create()
        {
            var herramientasDisponibles = _context.Herramientas
                .Where(h => h.Estado == "Disponible")
                .Select(h => new { h.IdHerramienta, DisplayText = h.Nombre + " (" + h.CodigoInterno + ")" })
                .ToList();

            var trabajadoresRegistrados = _context.Trabajadores
                .Select(t => new { t.IdTrabajador, DisplayText = t.Nombres + " (" + t.Cargo + ")" })
                .ToList();

            ViewBag.IdHerramienta = new SelectList(herramientasDisponibles, "IdHerramienta", "DisplayText");
            ViewBag.IdTrabajador = new SelectList(trabajadoresRegistrados, "IdTrabajador", "DisplayText");

            return View();
        }

        // POST: Prestamoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHerramienta,IdTrabajador,Observacion")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                prestamo.FechaPrestamo = DateTime.Now;
                prestamo.FechaDevolucion = DateTime.Now.AddMonths(1);
                prestamo.Estado = "Activo";

                var herramienta = await _context.Herramientas.FindAsync(prestamo.IdHerramienta);
                if (herramienta != null)
                    herramienta.Estado = "Prestada";

                _context.Add(prestamo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var herramientasDisponibles = _context.Herramientas
                .Where(h => h.Estado == "Disponible")
                .Select(h => new { h.IdHerramienta, DisplayText = h.Nombre + " (" + h.CodigoInterno + ")" })
                .ToList();

            var trabajadoresRegistrados = _context.Trabajadores
                .Select(t => new { t.IdTrabajador, DisplayText = t.Nombres + " (" + t.Cargo + ")" })
                .ToList();

            ViewBag.IdHerramienta = new SelectList(herramientasDisponibles, "IdHerramienta", "DisplayText", prestamo.IdHerramienta);
            ViewBag.IdTrabajador = new SelectList(trabajadoresRegistrados, "IdTrabajador", "DisplayText", prestamo.IdTrabajador);

            return View(prestamo);
        }

        // GET: Prestamoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var prestamo = await _context.Prestamos
                .Include(p => p.Herramienta)
                .Include(p => p.Trabajador)
                .FirstOrDefaultAsync(p => p.IdPrestamo == id);

            if (prestamo == null)
                return NotFound();

            return View(prestamo);
        }

        // POST: Prestamoes/Edit/5 → Marcar como devuelto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPrestamo,Observacion")] Prestamo prestamo)
        {
            if (id != prestamo.IdPrestamo)
                return NotFound();

            var prestamoDb = await _context.Prestamos
                .Include(p => p.Herramienta)
                .FirstOrDefaultAsync(p => p.IdPrestamo == id);

            if (prestamoDb == null)
                return NotFound();

            try
            {
                prestamoDb.Observacion = prestamo.Observacion;
                prestamoDb.FechaDevolucion = DateTime.Now;
                prestamoDb.Estado = "Devuelto";

                if (prestamoDb.Herramienta != null)
                    prestamoDb.Herramienta.Estado = "Disponible";

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrestamoExists(prestamo.IdPrestamo))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Prestamoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var prestamo = await _context.Prestamos
                .Include(p => p.Herramienta)
                .Include(p => p.Trabajador)
                .FirstOrDefaultAsync(m => m.IdPrestamo == id);

            if (prestamo == null)
                return NotFound();

            return View(prestamo);
        }

        // POST: Prestamoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
                _context.Prestamos.Remove(prestamo);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.IdPrestamo == id);
        }
    }
}
