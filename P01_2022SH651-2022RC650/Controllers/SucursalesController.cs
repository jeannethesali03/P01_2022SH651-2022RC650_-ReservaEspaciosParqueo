using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022SH651_2022RC650.Models;
using Microsoft.EntityFrameworkCore;

//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

namespace P01_2022SH651_2022RC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalesController : ControllerBase
    {
        private readonly ParqueoContext _parqueoContext;

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Sucursales> listadoSucursales = (from Sucursales in _parqueoContext.Sucursales select Sucursales).ToList();

            if (listadoSucursales.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoSucursales);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarSucursal([FromBody] Sucursales sucursal)
        {
            try
            {
                _parqueoContext.Sucursales.Add(sucursal);
                _parqueoContext.SaveChanges();
                return Ok(sucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizarSucursal/{id}")]
        public IActionResult ActualizarSucursal(int id, [FromBody] Sucursales sucursalModificar)
        {
            Sucursales? sucursalActual = (from Sucursales in _parqueoContext.Sucursales
                                          where Sucursales.Id_Sucursal == id
                                          select Sucursales).FirstOrDefault();

            if (sucursalActual == null)
            {
                return NotFound();
            }

            sucursalActual.Nombre = sucursalModificar.Nombre;
            sucursalActual.Direccion = sucursalModificar.Direccion;
            sucursalActual.Telefono = sucursalModificar.Telefono;
            sucursalActual.Id_Usuario = sucursalModificar.Id_Usuario;
            sucursalActual.Numero_Espacios = sucursalModificar.Numero_Espacios;

            _parqueoContext.Entry(sucursalActual).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(sucursalModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarSucursal(int id)
        {
            Sucursales? sucursal = (from Sucursales in _parqueoContext.Sucursales
                                    where Sucursales.Id_Sucursal == id
                                    select Sucursales).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound();
            }

            _parqueoContext.Sucursales.Attach(sucursal);
            _parqueoContext.Sucursales.Remove(sucursal);
            _parqueoContext.SaveChanges();

            return Ok(sucursal);
        }
    }
}
