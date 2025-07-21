using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolTrack.Domain.Entities;
using ToolTrack.Infrastructure.Persistence;

namespace ToolTrack.Web.Controllers
{
    public class CategoriaHerramientasController : Controller
    {
        private readonly ToolTrackDbContext _context;

        public CategoriaHerramientasController(ToolTrackDbContext context)
        {
            _context = context;
        }

        // GET: CategoriaHerramientas
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriasHerramienta.ToListAsync());
        }

        // GET: CategoriaHerramientas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaHerramienta = await _context.CategoriasHerramienta
                .FirstOrDefaultAsync(m => m.IdCategoria == id);

            if (categoriaHerramienta == null)
            {
                return NotFound();
            }

            return View(categoriaHerramienta);
        }

        // GET: CategoriaHerramientas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaHerramientas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,Nombre,Descripcion")] CategoriaHerramienta categoriaHerramienta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaHerramienta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoriaHerramienta);
        }

        // GET: CategoriaHerramientas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaHerramienta = await _context.CategoriasHerramienta.FindAsync(id);
            if (categoriaHerramienta == null)
            {
                return NotFound();
            }

            return View(categoriaHerramienta);
        }

        // POST: CategoriaHerramientas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoria,Nombre,Descripcion")] CategoriaHerramienta categoriaHerramienta)
        {
            if (id != categoriaHerramienta.IdCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaHerramienta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaHerramientaExists(categoriaHerramienta.IdCategoria))
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

            return View(categoriaHerramienta);
        }

        // GET: CategoriaHerramientas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaHerramienta = await _context.CategoriasHerramienta
                .FirstOrDefaultAsync(m => m.IdCategoria == id);

            if (categoriaHerramienta == null)
            {
                return NotFound();
            }

            return View(categoriaHerramienta);
        }

        // POST: CategoriaHerramientas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriaHerramienta = await _context.CategoriasHerramienta.FindAsync(id);
            if (categoriaHerramienta != null)
            {
                _context.CategoriasHerramienta.Remove(categoriaHerramienta);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaHerramientaExists(int id)
        {
            return _context.CategoriasHerramienta.Any(e => e.IdCategoria == id);
        }
    }
}
