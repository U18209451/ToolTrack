using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolTrack.Domain.Entities;
using ToolTrack.Infrastructure.Persistence;

namespace ToolTrack.Web.Controllers
{
    public class UbicacionsController : Controller
    {
        private readonly ToolTrackDbContext _context;

        public UbicacionsController(ToolTrackDbContext context)
        {
            _context = context;
        }

        // GET: Ubicacions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ubicaciones.ToListAsync());
        }

        // GET: Ubicacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubicacion = await _context.Ubicaciones
                .FirstOrDefaultAsync(m => m.IdUbicacion == id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            return View(ubicacion);
        }

        // GET: Ubicacions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ubicacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUbicacion,Nombre,Descripcion")] Ubicacion ubicacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ubicacion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ubicacion);
        }

        // GET: Ubicacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            return View(ubicacion);
        }

        // POST: Ubicacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUbicacion,Nombre,Descripcion")] Ubicacion ubicacion)
        {
            if (id != ubicacion.IdUbicacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ubicacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UbicacionExists(ubicacion.IdUbicacion))
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

            return View(ubicacion);
        }

        // GET: Ubicacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubicacion = await _context.Ubicaciones
                .FirstOrDefaultAsync(m => m.IdUbicacion == id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            return View(ubicacion);
        }

        // POST: Ubicacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion != null)
            {
                _context.Ubicaciones.Remove(ubicacion);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UbicacionExists(int id)
        {
            return _context.Ubicaciones.Any(e => e.IdUbicacion == id);
        }
    }
}
