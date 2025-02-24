using Microsoft.EntityFrameworkCore;

//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

namespace P01_2022SH651_2022RC650.Models
{
    public class ParqueoContext:DbContext
    {
        public ParqueoContext(DbContextOptions<ParqueoContext> options) : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Espacios_Parqueo> Espacios_Parqueo { get; set; }
        public DbSet<Sucursales> Sucursales { get; set; }
    }
}
