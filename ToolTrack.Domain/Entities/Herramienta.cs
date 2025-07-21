using System.ComponentModel.DataAnnotations;

namespace ToolTrack.Domain.Entities
{
    public class Herramienta
    {
        [Key]
        public int IdHerramienta { get; set; }
        public string Nombre { get; set; } = null!;
        public string CodigoInterno { get; set; } = null!;
        public string Estado { get; set; } = "Disponible";
        public DateTime FechaRegistro { get; set; }

        public int IdCategoria { get; set; }
        public CategoriaHerramienta? Categoria { get; set; }

        public int IdUbicacion { get; set; }
        public Ubicacion? Ubicacion { get; set; }

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
