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
        public SucursalesController(ParqueoContext parqueoContexto)
        {
            _parqueoContext = parqueoContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var listadoSucursales = (from s in _parqueoContext.Sucursales select new { 
                s.Nombre,
                s.Direccion,
                s.Telefono,
                s.Numero_Espacios,
                Administradores = (from u in _parqueoContext.Usuarios where s.Id_Usuario == u.Id_Usuario select new {u.Nombre, u.Correo, u.Rol}).ToList(),
                Espacios = (from e in _parqueoContext.Espacios_Parqueo where s.Id_Sucursal == e.Id_Sucursal select new { e.Numero_Espacio, e.Ubicacion, e.Costo_Por_Hora }).ToList()

            }).ToList();

            if (listadoSucursales.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoSucursales);
        }

        [HttpGet]
        [Route("GetById{id}")]
        public IActionResult Get(int id)
        {
            var listadoSucursales = (from s in _parqueoContext.Sucursales
                                     where s.Id_Sucursal == id
                                     select new
                                     {
                                         s.Nombre,
                                         s.Direccion,
                                         s.Telefono,
                                         s.Numero_Espacios,
                                         Administradores = (from u in _parqueoContext.Usuarios select new { u.Nombre, u.Correo, u.Rol }),
                                         Espacios = (from e in _parqueoContext.Espacios_Parqueo select new { e.Numero_Espacio, e.Ubicacion, e.Costo_Por_Hora })
                                     }).ToList();

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
