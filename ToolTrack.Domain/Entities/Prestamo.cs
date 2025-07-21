using System.ComponentModel.DataAnnotations;

namespace ToolTrack.Domain.Entities
{
    public class Prestamo
    {
        [Key]
        public int IdPrestamo { get; set; }

        public int IdHerramienta { get; set; }
        public Herramienta? Herramienta { get; set; }

        public int IdTrabajador { get; set; }
        public Trabajador? Trabajador { get; set; }

        public DateTime FechaPrestamo { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public string Estado { get; set; } = "Activo";
        public string? Observacion { get; set; }
    }
}
