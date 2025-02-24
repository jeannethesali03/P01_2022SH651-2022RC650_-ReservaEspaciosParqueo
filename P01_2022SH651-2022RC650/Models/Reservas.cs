//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

using System.ComponentModel.DataAnnotations;

namespace P01_2022SH651_2022RC650.Models
{
    public class Reservas
    {
        [Key]
        public int Id_Reserva { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Espacio { get; set; }
        public DateTime Fecha_Reserva { get; set; }
        public TimeSpan Hora_Reserva { get; set; }
        public int Cantidad_Horas { get; set; }
        public string Estado { get; set; }
    }
}
