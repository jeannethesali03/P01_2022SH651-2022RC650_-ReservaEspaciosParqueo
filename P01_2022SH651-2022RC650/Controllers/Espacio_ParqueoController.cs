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
        [Route("GetEspaciosDisponibles/{id}")]
        public IActionResult GetEspaciosDisponibles()
        {
            List<Espacios_Parqueo> listadoEspacios = (from Espacios_Parqueo in _parqueoContext.Espacios_Parqueo
                                                      where Espacios_Parqueo.Estado == "Disponible"
                                                      select Espacios_Parqueo).ToList();
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
            espacioActual.Estado = espacioModificar.Estado;
            espacioActual.Id_Sucursal = espacioModificar.Id_Sucursal;

            _parqueoContext.Entry(espacioActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();
            return Ok(espacioModificar);
        }
    }
}
