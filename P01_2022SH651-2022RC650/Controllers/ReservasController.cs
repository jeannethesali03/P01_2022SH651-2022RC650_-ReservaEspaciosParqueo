using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022SH651_2022RC650.Models;

//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

namespace P01_2022SH651_2022RC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ParqueoContext _parqueoContext;
        public ReservasController(ParqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }



        [HttpPost]
        [Route("Reservar")]
        public IActionResult Reservar(string correo, string contrasena, int idEspacio, DateTime fechaReserva, TimeSpan horaReserva, int cantidadHoras)
        {
            try
            {
                // Verificar si el usuario existe antes de proceder con la reserva
                var usuario = _parqueoContext.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);

                if (usuario == null)
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }

                // Crear la reserva
                var nuevaReserva = new Reservas
                {
                    Id_Usuario = usuario.Id_Usuario,
                    Id_Espacio = idEspacio,
                    Fecha_Reserva = fechaReserva,
                    Hora_Reserva = horaReserva,
                    Cantidad_Horas = cantidadHoras,
                    Estado = "Activa"
                };

                _parqueoContext.Reservas.Add(nuevaReserva);
                _parqueoContext.SaveChanges();

                return Ok("Reserva realizada con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("ReservasActivasPorUsuario")]
        public IActionResult ObtenerReservasActivas(string correo, string contrasena)
        {
            try
            {
                // Verificar si el usuario existe
                var usuario = _parqueoContext.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);
                if (usuario == null)
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }

                // Obtener las reservas activas del usuario
                var reservas = _parqueoContext.Reservas
                    .Where(r => r.Id_Usuario == usuario.Id_Usuario && r.Estado == "Reservado")
                    .Select(r => new
                    {
                        r.Id_Reserva,
                        r.Fecha_Reserva,
                        r.Hora_Reserva,
                        r.Cantidad_Horas,
                        r.Estado,
                        Espacio = _parqueoContext.Espacios_Parqueo
                            .Where(e => e.Id_Espacio == r.Id_Espacio)
                            .Select(e => new
                            {
                                e.Id_Espacio,
                                e.Numero_Espacio,
                                e.Ubicacion
                            })
                            .FirstOrDefault()
                    })
                    .ToList();

                if (reservas.Count == 0)
                {
                    return NotFound("No hay reservas activas.");
                }

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("CancelarReserva")]
        public IActionResult CancelarReserva(string correo, string contrasena, int idReserva)
        {
            try
            {
                var usuario = _parqueoContext.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);
                if (usuario == null)
                {
                    return Unauthorized("Correo o contraseña incorrectos.");
                }

                var reserva = _parqueoContext.Reservas.FirstOrDefault(r => r.Id_Reserva == idReserva && r.Id_Usuario == usuario.Id_Usuario);
                if (reserva == null)
                {
                    return NotFound("Reserva no encontrada.");
                }

                // Verificar que la reserva aún no haya sido usada
                if (reserva.Fecha_Reserva <= DateTime.Now.Date && reserva.Hora_Reserva <= DateTime.Now.TimeOfDay)
                {
                    return BadRequest("No se puede cancelar una reserva ya iniciada o pasada.");
                }

                // Marcar reserva como cancelada
                var aaaa = _parqueoContext.Reservas.FirstOrDefault(e => e.Id_Reserva == reserva.Id_Reserva);
                if (aaaa != null)
                {
                    aaaa.Estado = "Cancelada";
                    _parqueoContext.SaveChanges(); // Guardar cambios en la base de datos
                }



                return Ok("Reserva cancelada exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("EspaciosReservadosPorDia")]
        public IActionResult ObtenerEspaciosReservadosPorDia(DateTime fecha)
        {
            try
            {
                var reservas = _parqueoContext.Reservas
                    .Where(r => r.Fecha_Reserva == fecha)
                    .Select(r => new
                    {
                        r.Id_Reserva,
                        r.Fecha_Reserva,
                        r.Hora_Reserva,
                        r.Cantidad_Horas,
                        r.Estado,
                        Espacio = _parqueoContext.Espacios_Parqueo
                            .Where(e => e.Id_Espacio == r.Id_Espacio)
                            .Select(e => new
                            {
                                e.Id_Espacio,
                                e.Numero_Espacio,
                                e.Ubicacion,
                                Sucursal = _parqueoContext.Sucursales
                                    .Where(s => s.Id_Sucursal == e.Id_Sucursal)
                                    .Select(s => new
                                    {
                                        s.Id_Sucursal,
                                        s.Nombre,
                                        s.Direccion
                                    })
                                    .FirstOrDefault()
                            })
                            .FirstOrDefault()
                    })
                    .ToList();

                if (reservas.Count == 0)
                {
                    return NotFound("No hay reservas registradas para esta fecha.");
                }

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("EspaciosReservadosEntreFechas")]
        public IActionResult ObtenerEspaciosReservadosEntreFechas(int idSucursal, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var espaciosReservados = _parqueoContext.Reservas
                    .Where(r => r.Fecha_Reserva >= fechaInicio && r.Fecha_Reserva <= fechaFin)
                    .Where(r => r.Estado == "Activa")  // Filtra las reservas activas
                    .Join(
                        _parqueoContext.Espacios_Parqueo,
                        r => r.Id_Espacio,
                        e => e.Id_Espacio,
                        (r, e) => new { r, e }
                    )
                    .Where(re => re.e.Id_Sucursal == idSucursal)
                    .Select(re => new
                    {
                        re.e.Numero_Espacio,    // Muestra el número del espacio
                        re.e.Ubicacion          // Muestra la ubicación del espacio
                    })
                    .Distinct() // Asegura que no se repitan los espacios reservados
                    .ToList();

                if (espaciosReservados.Count == 0)
                {
                    return NotFound("No hay espacios reservados para estas fechas en esta sucursal.");
                }

                return Ok(espaciosReservados);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
