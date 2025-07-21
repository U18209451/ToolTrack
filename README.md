Requerimientos:
- .NET 8
- EntityFrameworkCore.Tools

Setup:
- Opción PackageManager: En la ventana de PackageManager ejecutar "Update-Database" con el proyecto ToolTrack.Insfrastructure seleccionado
- Opción DotNet CLI: En el "Developer Tools", "Windows CMD", "PowerShell" o consola similar, navegar a la carpeta que contenga la solución y usar "dotnet ef database update --project TrackTool.Infrastructure"