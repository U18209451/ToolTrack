using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToolTrack.Domain.Entities;
using ToolTrack.Infrastructure.Persistence;

namespace ToolTrack.Web.Controllers
{
    public class HerramientasController : Controller
    {
        private readonly ToolTrackDbContext _context;

        public HerramientasController(ToolTrackDbContext context)
        {
            _context = context;
        }

        // GET: Herramientas
        public async Task<IActionResult> Index()
        {
            var toolTrackDbContext = _context.Herramientas.Include(h => h.Categoria).Include(h => h.Ubicacion);
            return View(await toolTrackDbContext.ToListAsync());
        }

        // GET: Herramientas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var herramienta = await _context.Herramientas
                .Include(h => h.Categoria)
                .Include(h => h.Ubicacion)
                .FirstOrDefaultAsync(m => m.IdHerramienta == id);
            if (herramienta == null)
            {
                return NotFound();
            }

            return View(herramienta);
        }

        // GET: Herramientas/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.CategoriasHerramienta, "IdCategoria", "Nombre");
            ViewData["IdUbicacion"] = new SelectList(_context.Ubicaciones, "IdUbicacion", "Nombre");
            return View();
        }

        // POST: Herramientas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHerramienta,Nombre,CodigoInterno,Estado,IdCategoria,IdUbicacion")] Herramienta herramienta)
        {
            if (ModelState.IsValid)
            {
                herramienta.FechaRegistro = DateTime.UtcNow; // ⬅ Aquí le asignamos la fecha automáticamente
                _context.Add(herramienta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.CategoriasHerramienta, "IdCategoria", "Nombre", herramienta.IdCategoria);
            ViewData["IdUbicacion"] = new SelectList(_context.Ubicaciones, "IdUbicacion", "Nombre", herramienta.IdUbicacion);
            return View(herramienta);
        }

        // GET: Herramientas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var herramienta = await _context.Herramientas.FindAsync(id);
            if (herramienta == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.CategoriasHerramienta, "IdCategoria", "Nombre", herramienta.IdCategoria);
            ViewData["IdUbicacion"] = new SelectList(_context.Ubicaciones, "IdUbicacion", "Nombre", herramienta.IdUbicacion);
            return View(herramienta);
        }

        // POST: Herramientas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHerramienta,Nombre,CodigoInterno,Estado,FechaRegistro,IdCategoria,IdUbicacion")] Herramienta herramienta)
        {
            if (id != herramienta.IdHerramienta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(herramienta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HerramientaExists(herramienta.IdHerramienta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.CategoriasHerramienta, "IdCategoria", "Nombre", herramienta.IdCategoria);
            ViewData["IdUbicacion"] = new SelectList(_context.Ubicaciones, "IdUbicacion", "Nombre", herramienta.IdUbicacion);
            return View(herramienta);
        }

        // GET: Herramientas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var herramienta = await _context.Herramientas
                .Include(h => h.Categoria)
                .Include(h => h.Ubicacion)
                .FirstOrDefaultAsync(m => m.IdHerramienta == id);
            if (herramienta == null)
            {
                return NotFound();
            }

            return View(herramienta);
        }

        // POST: Herramientas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var herramienta = await _context.Herramientas.FindAsync(id);
            if (herramienta != null)
            {
                _context.Herramientas.Remove(herramienta);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool HerramientaExists(int id)
        {
            return _context.Herramientas.Any(e => e.IdHerramienta == id);
        }
    }
}
