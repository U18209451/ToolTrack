using System.ComponentModel.DataAnnotations;

namespace ToolTrack.Domain.Entities
{
    public class CategoriaHerramienta
    {
        [Key]
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        public ICollection<Herramienta> Herramientas { get; set; } = new List<Herramienta>();
    }
}
