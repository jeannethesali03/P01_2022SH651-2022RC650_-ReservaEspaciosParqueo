using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022SH651_2022RC650.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

//Eyleen Jeannethe Salinas Hernández
//Wilber Anibal Rivas Carranza

namespace P01_2022SH651_2022RC650.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ParqueoContext _parqueoContext;
        public UsuariosController(ParqueoContext parqueoContext)
        {
            _parqueoContext = parqueoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Usuarios> listadoUsuario = (from Usuarios in _parqueoContext.Usuarios select Usuarios).ToList();

            if (listadoUsuario.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuario);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _parqueoContext.Usuarios.FirstOrDefault(u => u.Id_Usuario == id);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(usuario);
        }



        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] Usuarios usuario)
        {
            try
            {
                // Validar formato de correo
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(usuario.Correo, emailPattern))
                {
                    return BadRequest("El formato del correo es inválido.");
                }

                // Validar longitud de la contraseña
                if (usuario.Contrasena.Length < 6)
                {
                    return BadRequest("La contraseña debe tener al menos 6 caracteres.");
                }

                _parqueoContext.Usuarios.Add(usuario);
                _parqueoContext.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Usuarios usuarioModificar)
        {
            Usuarios? usuarioActual = (from Usuarios in _parqueoContext.Usuarios
                                       where Usuarios.Id_Usuario == id
                                       select Usuarios).FirstOrDefault();

            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.Nombre = usuarioModificar.Nombre;
            usuarioActual.Correo = usuarioModificar.Correo;
            usuarioActual.Telefono = usuarioModificar.Telefono;
            usuarioActual.Contrasena = usuarioModificar.Contrasena;
            usuarioActual.Rol = usuarioModificar.Rol;

            _parqueoContext.Entry(usuarioActual).State = EntityState.Modified;
            _parqueoContext.SaveChanges();

            return Ok(usuarioModificar);
        }


        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            Usuarios? usuarios = (from Usuarios in _parqueoContext.Usuarios
                                  where Usuarios.Id_Usuario == id
                                  select Usuarios).FirstOrDefault();

            if (usuarios == null)
            {
                return NotFound();
            }

            _parqueoContext.Usuarios.Attach(usuarios);
            _parqueoContext.Usuarios.Remove(usuarios);
            _parqueoContext.SaveChanges();

            return Ok(usuarios);
        }




    }
}
