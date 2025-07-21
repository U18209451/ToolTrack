using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToolTrack.Domain.Entities;
using ToolTrack.Infrastructure.Persistence;

namespace ToolTrack.Web.Controllers
{
    public class BotController : Controller
    {
        private readonly ToolTrackDbContext _context;

        private static string BotState = "";
        private static int SelectedTrabajadorId = 0;
        private static int SelectedHerramientaId = 0;

        public BotController(ToolTrackDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetResponse(string userMessage)
        {
            string response = "";

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                response = "❌ No entendí tu mensaje. Por favor, intenta de nuevo.";
            }
            else if (userMessage.ToLower().Contains("prestamo") && BotState == "")
            {
                BotState = "SeleccionTrabajador";

                var trabajadores = await _context.Trabajadores.ToListAsync();
                response = "✅ ¡Vamos a hacer un préstamo! Estos son los trabajadores:\n";

                for (int i = 0; i < trabajadores.Count; i++)
                {
                    response += $"{i + 1}. {trabajadores[i].Nombres} ({trabajadores[i].Cargo})\n";
                }

                response += "👉 Por favor, responde con el número del trabajador.";
            }
            else if (BotState == "SeleccionTrabajador" && int.TryParse(userMessage, out int trabajadorIndex))
            {
                var trabajadores = await _context.Trabajadores.ToListAsync();

                if (trabajadorIndex >= 1 && trabajadorIndex <= trabajadores.Count)
                {
                    SelectedTrabajadorId = trabajadores[trabajadorIndex - 1].IdTrabajador;
                    BotState = "SeleccionHerramienta";

                    var herramientas = await _context.Herramientas.Where(h => h.Estado == "Disponible").ToListAsync();
                    response = $"✅ Trabajador seleccionado: {trabajadores[trabajadorIndex - 1].Nombres}\n";
                    response += "Estas son las herramientas disponibles:\n";

                    for (int i = 0; i < herramientas.Count; i++)
                    {
                        response += $"{i + 1}. {herramientas[i].Nombre} ({herramientas[i].CodigoInterno})\n";
                    }

                    response += "👉 Por favor, responde con el número de la herramienta.";
                }
                else
                {
                    response = "❌ Número inválido. Intenta de nuevo.";
                }
            }
            else if (BotState == "SeleccionHerramienta" && int.TryParse(userMessage, out int herramientaIndex))
            {
                var herramientas = await _context.Herramientas.Where(h => h.Estado == "Disponible").ToListAsync();

                if (herramientaIndex >= 1 && herramientaIndex <= herramientas.Count)
                {
                    SelectedHerramientaId = herramientas[herramientaIndex - 1].IdHerramienta;

                    var prestamo = new Prestamo
                    {
                        IdHerramienta = SelectedHerramientaId,
                        IdTrabajador = SelectedTrabajadorId,
                        FechaPrestamo = DateTime.Now,
                        FechaDevolucion = DateTime.Now.AddMonths(1),
                        Estado = "Activo"
                    };

                    herramientas[herramientaIndex - 1].Estado = "Prestada";
                    _context.Prestamos.Add(prestamo);
                    await _context.SaveChangesAsync();

                    response = $"✅ ¡Préstamo registrado!\n Herramienta: {herramientas[herramientaIndex - 1].Nombre} para el trabajador seleccionado.";
                    BotState = "";
                }
                else
                {
                    response = "❌ Número inválido. Intenta de nuevo.";
                }
            }
            else if (userMessage.ToLower().Contains("devolver") && BotState == "")
            {
                BotState = "SeleccionDevolucion";

                var prestamosActivos = await _context.Prestamos
                    .Include(p => p.Herramienta)
                    .Include(p => p.Trabajador)
                    .Where(p => p.Estado == "Activo")
                    .ToListAsync();

                if (!prestamosActivos.Any())
                {
                    response = "📦 No hay préstamos activos para devolver.";
                    BotState = "";
                }
                else
                {
                    response = "✅ Estos son los préstamos activos:\n";
                    for (int i = 0; i < prestamosActivos.Count; i++)
                    {
                        response += $"{i + 1}. {prestamosActivos[i].Herramienta?.Nombre} para {prestamosActivos[i].Trabajador?.Nombres}\n";
                    }
                    response += "👉 Por favor, responde con el número del préstamo a devolver.";
                }
            }
            else if (BotState == "SeleccionDevolucion" && int.TryParse(userMessage, out int prestamoIndex))
            {
                var prestamosActivos = await _context.Prestamos
                    .Include(p => p.Herramienta)
                    .Include(p => p.Trabajador)
                    .Where(p => p.Estado == "Activo")
                    .ToListAsync();

                if (prestamoIndex >= 1 && prestamoIndex <= prestamosActivos.Count)
                {
                    var prestamo = prestamosActivos[prestamoIndex - 1];
                    prestamo.Estado = "Devuelto";
                    prestamo.FechaDevolucion = DateTime.Now;
                    if (prestamo.Herramienta != null)
                        prestamo.Herramienta.Estado = "Disponible";

                    await _context.SaveChangesAsync();

                    response = $"✅ ¡Préstamo devuelto!\n Herramienta: {prestamo.Herramienta?.Nombre} del trabajador {prestamo.Trabajador?.Nombres}.";
                    BotState = "";
                }
                else
                {
                    response = "❌ Número inválido. Intenta de nuevo.";
                }
            }
            else
            {
                response = "🤖 Hola, puedes decir:\n- 'prestamo' para registrar préstamo\n- 'devolver' para devolver herramienta.";
            }

            return Json(new { reply = response.Replace("\n", "<br>") });
        }
    }
}
