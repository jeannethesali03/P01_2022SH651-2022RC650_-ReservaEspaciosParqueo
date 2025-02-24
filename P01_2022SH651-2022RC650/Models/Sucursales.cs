//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

using System.ComponentModel.DataAnnotations;

namespace P01_2022SH651_2022RC650.Models
{
    public class Sucursales
    {
        [Key]
        public int Id_Sucursal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int Id_Usuario { get; set; }
        public int Numero_Espacios { get; set; }
    }
}
