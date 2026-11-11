namespace BackendFotosApi.Models
{
    public class Fotografia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}