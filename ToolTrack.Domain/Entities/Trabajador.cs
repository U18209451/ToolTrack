using System.ComponentModel.DataAnnotations;

namespace ToolTrack.Domain.Entities
{
    public class Trabajador
    {
        [Key]
        public int IdTrabajador { get; set; }
        public string Nombres { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string Cargo { get; set; } = null!;

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
