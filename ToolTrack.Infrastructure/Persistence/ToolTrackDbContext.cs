using Microsoft.EntityFrameworkCore;
using ToolTrack.Domain.Entities;

namespace ToolTrack.Infrastructure.Persistence
{
    public class ToolTrackDbContext : DbContext
    {
        public ToolTrackDbContext(DbContextOptions<ToolTrackDbContext> options) : base(options) { }

        public DbSet<Herramienta> Herramientas => Set<Herramienta>();
        public DbSet<Trabajador> Trabajadores => Set<Trabajador>();
        public DbSet<Prestamo> Prestamos => Set<Prestamo>();
        public DbSet<UsuarioSistema> UsuariosSistema => Set<UsuarioSistema>();
        public DbSet<CategoriaHerramienta> CategoriasHerramienta => Set<CategoriaHerramienta>();
        public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Herramienta -> Categoria
            modelBuilder.Entity<Herramienta>()
                .HasOne(h => h.Categoria)
                .WithMany(c => c.Herramientas)
                .HasForeignKey(h => h.IdCategoria);

            // Herramienta -> Ubicacion
            modelBuilder.Entity<Herramienta>()
                .HasOne(h => h.Ubicacion)
                .WithMany(u => u.Herramientas)
                .HasForeignKey(h => h.IdUbicacion);

            // Prestamo -> Herramienta
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Herramienta)
                .WithMany(h => h.Prestamos)
                .HasForeignKey(p => p.IdHerramienta);

            // Prestamo -> Trabajador
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Trabajador)
                .WithMany(t => t.Prestamos)
                .HasForeignKey(p => p.IdTrabajador);

            // SEED DATA

            // Categorías
            modelBuilder.Entity<CategoriaHerramienta>().HasData(
                new CategoriaHerramienta { IdCategoria = 1, Nombre = "Taladros", Descripcion = "Herramientas de perforación" },
                new CategoriaHerramienta { IdCategoria = 2, Nombre = "Martillos", Descripcion = "Herramientas de golpe" },
                new CategoriaHerramienta { IdCategoria = 3, Nombre = "Destornilladores", Descripcion = "Herramientas de ajuste" }
            );

            // Ubicaciones
            modelBuilder.Entity<Ubicacion>().HasData(
                new Ubicacion { IdUbicacion = 1, Nombre = "Almacén Central", Descripcion = "Principal" },
                new Ubicacion { IdUbicacion = 2, Nombre = "Almacén Secundario", Descripcion = "Secundario" },
                new Ubicacion { IdUbicacion = 3, Nombre = "Taller Norte", Descripcion = "Zona Norte" }
            );

            // Trabajadores
            modelBuilder.Entity<Trabajador>().HasData(
                new Trabajador { IdTrabajador = 1, Nombres = "Juan Pérez", Documento = "12345678", Cargo = "Operario" },
                new Trabajador { IdTrabajador = 2, Nombres = "Ana Gómez", Documento = "87654321", Cargo = "Supervisor" },
                new Trabajador { IdTrabajador = 3, Nombres = "Gerson García", Documento = "47134232", Cargo = "Administrador" },
                new Trabajador { IdTrabajador = 4, Nombres = "María López", Documento = "55555555", Cargo = "Técnico" }
            );

            // Usuarios del sistema
            modelBuilder.Entity<UsuarioSistema>().HasData(
                new UsuarioSistema { IdUsuario = 1, NombreUsuario = "admin", ClaveHash = "admin123", Rol = "Admin", Activo = true },
                new UsuarioSistema { IdUsuario = 2, NombreUsuario = "user", ClaveHash = "user123", Rol = "User", Activo = true }
            );

            // Herramientas
            modelBuilder.Entity<Herramienta>().HasData(
                new Herramienta
                {
                    IdHerramienta = 1,
                    Nombre = "Taladro Bosch",
                    CodigoInterno = "TB500",
                    Estado = "Disponible",
                    FechaRegistro = DateTime.UtcNow,
                    IdCategoria = 1,
                    IdUbicacion = 1
                },
                new Herramienta
                {
                    IdHerramienta = 2,
                    Nombre = "Martillo Stanley",
                    CodigoInterno = "MS100",
                    Estado = "Disponible",
                    FechaRegistro = DateTime.UtcNow,
                    IdCategoria = 2,
                    IdUbicacion = 2
                },
                new Herramienta
                {
                    IdHerramienta = 3,
                    Nombre = "Destornillador Philips",
                    CodigoInterno = "DP200",
                    Estado = "Disponible",
                    FechaRegistro = DateTime.UtcNow,
                    IdCategoria = 3,
                    IdUbicacion = 3
                },
                new Herramienta
                {
                    IdHerramienta = 4,
                    Nombre = "Taladro Makita",
                    CodigoInterno = "TM800",
                    Estado = "Disponible",
                    FechaRegistro = DateTime.UtcNow,
                    IdCategoria = 1,
                    IdUbicacion = 1
                }
            );
        }
    }
}
