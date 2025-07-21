using System.ComponentModel.DataAnnotations;

namespace ToolTrack.Domain.Entities
{
    public class UsuarioSistema
    {
        [Key]
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string ClaveHash { get; set; } = null!;
        public string Rol { get; set; } = "User";
        public bool Activo { get; set; }
    }
}
