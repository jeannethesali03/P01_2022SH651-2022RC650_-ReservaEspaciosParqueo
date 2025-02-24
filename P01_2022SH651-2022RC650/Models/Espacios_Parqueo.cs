//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

using System.ComponentModel.DataAnnotations;

namespace P01_2022SH651_2022RC650.Models
{
    public class Espacios_Parqueo
    {
        [Key]
        public int Id_Espacio { get; set; }
        public int Id_Sucursal { get; set; }
        public string Numero_Espacio { get; set; }
        public string Ubicacion { get; set; }
        public decimal Costo_Por_Hora { get; set; }
        public string Estado { get; set; }
    }
}
