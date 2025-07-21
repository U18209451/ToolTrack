using System.ComponentModel.DataAnnotations;

namespace ToolTrack.Domain.Entities
{
    public class Ubicacion
    {
        [Key]
        public int IdUbicacion { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        public ICollection<Herramienta> Herramientas { get; set; } = new List<Herramienta>();
    }
}
