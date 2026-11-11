using BackendFotosApi.Data;
using BackendFotosApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Entity Framework Core con SQL Server utilizando la cadena de conexión del appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Forzar al servidor a escuchar peticiones en la red local (puerto 5188) para el celular físico
builder.WebHost.UseUrls("http://0.0.0.0:5188");

var app = builder.Build();

// 3. Endpoint POST para recibir la foto seleccionada y guardarla en la base de datos
app.MapPost("/api/fotos/upload", async (IFormFile file, AppDbContext context) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No se ha seleccionado ningún archivo de imagen.");
    }

    try
    {
        // Convertir el archivo IFormFile a un arreglo de bytes (varbinary para SQL Server)
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var bytesImagen = memoryStream.ToArray();

        // Crear la instancia de la entidad Fotografia
        var fotografia = new Fotografia
        {
            Nombre = file.FileName,
            ImageData = bytesImagen,
            FechaCreacion = DateTime.Now
        };

        // Guardar en la base de datos mediante Entity Framework Core
        context.Fotografias.Add(fotografia);
        await context.SaveChangesAsync();

        return Results.Ok(new { mensaje = "¡Fotografía guardada con éxito en SQL Server!", id = fotografia.Id });
    }
    catch (Exception ex)
    {
        // Retornar error interno en caso de fallos en el servidor o la BD
        return Results.StatusCode(500);
    }
})
.DisableAntiforgery(); // <-- Solución al error de anti-forgery metadata para peticiones móviles

app.Run();