namespace CHRISTIANEXAMENFINAL.Models
{
    public class EventoCorporativo
    {
        public int EventoID { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Ubicacion { get; set; }
        public int TipoEventoID { get; set; }
        public string Descripcion { get; set; }
        public TipoEvento TipoEvento { get; set; }  // Relación con TipoEvento
    }
}
