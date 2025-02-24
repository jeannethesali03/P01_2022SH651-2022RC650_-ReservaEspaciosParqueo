//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

using System.ComponentModel.DataAnnotations;

namespace P01_2022SH651_2022RC650.Models
{
    public class Usuarios
    {
        [Key]
        public int Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }
    }
}
