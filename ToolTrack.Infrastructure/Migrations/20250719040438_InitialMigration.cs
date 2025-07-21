using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToolTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasHerramienta",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasHerramienta", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Trabajadores",
                columns: table => new
                {
                    IdTrabajador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trabajadores", x => x.IdTrabajador);
                });

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                columns: table => new
                {
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.IdUbicacion);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosSistema",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaveHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosSistema", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Herramientas",
                columns: table => new
                {
                    IdHerramienta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoInterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Herramientas", x => x.IdHerramienta);
                    table.ForeignKey(
                        name: "FK_Herramientas_CategoriasHerramienta_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriasHerramienta",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Herramientas_Ubicaciones_IdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    IdPrestamo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdHerramienta = table.Column<int>(type: "int", nullable: false),
                    IdTrabajador = table.Column<int>(type: "int", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.IdPrestamo);
                    table.ForeignKey(
                        name: "FK_Prestamos_Herramientas_IdHerramienta",
                        column: x => x.IdHerramienta,
                        principalTable: "Herramientas",
                        principalColumn: "IdHerramienta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prestamos_Trabajadores_IdTrabajador",
                        column: x => x.IdTrabajador,
                        principalTable: "Trabajadores",
                        principalColumn: "IdTrabajador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CategoriasHerramienta",
                columns: new[] { "IdCategoria", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Herramientas de perforación", "Taladros" },
                    { 2, "Herramientas de golpe", "Martillos" }
                });

            migrationBuilder.InsertData(
                table: "Trabajadores",
                columns: new[] { "IdTrabajador", "Cargo", "Documento", "Nombres" },
                values: new object[,]
                {
                    { 1, "Operario", "12345678", "Juan Pérez" },
                    { 2, "Supervisor", "87654321", "Ana Gómez" }
                });

            migrationBuilder.InsertData(
                table: "Ubicaciones",
                columns: new[] { "IdUbicacion", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Principal", "Almacén Central" },
                    { 2, "Secundario", "Almacén Secundario" }
                });

            migrationBuilder.InsertData(
                table: "UsuariosSistema",
                columns: new[] { "IdUsuario", "Activo", "ClaveHash", "NombreUsuario", "Rol" },
                values: new object[,]
                {
                    { 1, true, "admin123", "admin", "Admin" },
                    { 2, true, "user123", "user", "User" }
                });

            migrationBuilder.InsertData(
                table: "Herramientas",
                columns: new[] { "IdHerramienta", "CodigoInterno", "Estado", "FechaRegistro", "IdCategoria", "IdUbicacion", "Nombre" },
                values: new object[,]
                {
                    { 1, "TB500", "Disponible", new DateTime(2025, 7, 19, 4, 4, 37, 686, DateTimeKind.Utc).AddTicks(191), 1, 1, "Taladro Bosch" },
                    { 2, "MS100", "Disponible", new DateTime(2025, 7, 19, 4, 4, 37, 686, DateTimeKind.Utc).AddTicks(194), 2, 2, "Martillo Stanley" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Herramientas_IdCategoria",
                table: "Herramientas",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Herramientas_IdUbicacion",
                table: "Herramientas",
                column: "IdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_IdHerramienta",
                table: "Prestamos",
                column: "IdHerramienta");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_IdTrabajador",
                table: "Prestamos",
                column: "IdTrabajador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "UsuariosSistema");

            migrationBuilder.DropTable(
                name: "Herramientas");

            migrationBuilder.DropTable(
                name: "Trabajadores");

            migrationBuilder.DropTable(
                name: "CategoriasHerramienta");

            migrationBuilder.DropTable(
                name: "Ubicaciones");
        }
    }
}
