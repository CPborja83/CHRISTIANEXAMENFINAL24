namespace CHRISTIANEXAMENFINAL.Models
{
    public class AsistenteEvento
    {
        public int AsistenteID { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Rol { get; set; }
        public int EventoID { get; set; }
        public EventoCorporativo? Evento { get; set; }  // Relación con EventoCorporativo
    }
}
