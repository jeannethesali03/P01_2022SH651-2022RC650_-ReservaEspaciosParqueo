using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022SH651_2022RC650.Models;

//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

namespace P01_2022SH651_2022RC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Espacio_ParqueoController : ControllerBase
    {
        private readonly ParqueoContext _parqueoContext;
        public Espacio_ParqueoController(ParqueoContext parqueoContexto)
        {
            _parqueoContext = parqueoContexto;
        }

        [HttpPost]
        [Route("RegistrarEspacio")]
        public IActionResult GuardarEspacio([FromBody] Espacios_Parqueo espacio)
        {
            try
            {
                _parqueoContext.Espacios_Parqueo.Add(espacio);
                _parqueoContext.SaveChanges();
                return Ok(espacio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetEspaciosDisponibles")]
        public IActionResult GetEspaciosDisponibles(int sucursalId, DateTime fecha)
        {
            // Obtener los espacios reservados en la fecha especificada con estado "Activa"
            var espaciosReservados = _parqueoContext.Reservas
                .Where(r => r.Fecha_Reserva.Date == fecha.Date && r.Estado == "Activa")
                .Select(r => r.Id_Espacio)
                .ToList();

            // Obtener los espacios disponibles que no están reservados
            var listadoEspacios = (from e in _parqueoContext.Espacios_Parqueo
                                   where e.Id_Sucursal == sucursalId && !espaciosReservados.Contains(e.Id_Espacio)
                                   select new
                                   {
                                       e.Numero_Espacio,
                                       e.Ubicacion,
                                       e.Costo_Por_Hora,
                                       Sucursal = (from s in _parqueoContext.Sucursales
                                                   where s.Id_Sucursal == e.Id_Sucursal
                                                   select s.Nombre).FirstOrDefault()
                                   }).ToList();

            if (listadoEspacios.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoEspacios);
        }




        [HttpPut]
        [Route("actualizarEspacio/{id}")]
        public IActionResult ActualizarEspacio(int id, [FromBody] Espacios_Parqueo espacioModificar)
        {
            Espacios_Parqueo? espacioActual = (from Espacios_Parqueo in _parqueoContext.Espacios_Parqueo
                                               where Espacios_Parqueo.Id_Espacio == id
                                               select Espacios_Parqueo).FirstOrDefault();
            if (espacioActual == null)
            {
                return NotFound();
            }
            espacioActual.Numero_Espacio = espacioModificar.Numero_Espacio;
            espacioActual.Ubicacion = espacioModificar.Ubicacion;
            espacioActual.Costo_Por_Hora = espacioModificar.Costo_Por_Hora;
            espacioActual.Id_Sucursal = espacioModificar.Id_Sucursal;

            _parqueoContext.Entry(espacioActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();
            return Ok(espacioModificar);
        }

        [HttpDelete]
        [Route("eliminarEspacio/{id}")]
        public IActionResult EliminarEspacio(int id)
        {
            Espacios_Parqueo? espacio = (from Espacios_Parqueo in _parqueoContext.Espacios_Parqueo
                                         where Espacios_Parqueo.Id_Espacio == id
                                         select Espacios_Parqueo).FirstOrDefault();
            if (espacio == null)
            {
                return NotFound();
            }
            _parqueoContext.Espacios_Parqueo.Remove(espacio);
            _parqueoContext.SaveChanges();
            return Ok(espacio);
        }
    }
}
