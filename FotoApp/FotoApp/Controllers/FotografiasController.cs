using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FotoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotografiasController : ControllerBase
    {
        private readonly string _connectionString;

        public FotografiasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No se ha seleccionado ninguna imagen." });
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    using (var connection = new SqlConnection(_connectionString))
                    {
                        var query = "INSERT INTO Fotografias (NombreArchivo, ImagenData) VALUES (@NombreArchivo, @ImagenData)";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NombreArchivo", file.FileName);
                            command.Parameters.AddWithValue("@ImagenData", imageBytes);

                            await connection.OpenAsync();
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }

                return Ok(new { message = "Fotografía guardada exitosamente en SQL Server." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", error = ex.Message });
            }
        }
    }
}