using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_NUMEROS_CARNETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly blogDBContext _blogDBContext;

        public usuariosController(blogDBContext usuariosContexto)
        {
            _blogDBContext = usuariosContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuarios> listadoUsuarios = (from u in _blogDBContext.usuarios
                                          select u).ToList();

            if (listadoUsuarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoUsuarios);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarUsuario([FromBody] usuarios usuario)
        {
            try
            {
                _blogDBContext.usuarios.Add(usuario);
                _blogDBContext.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioModificar)
        {
            var usuarioActual = (from u in _blogDBContext.usuarios
                               where u.usuarioId == id
                               select u).FirstOrDefault();

            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.rolId = usuarioModificar.rolId;
            usuarioActual.nombreUsuario = usuarioModificar.nombreUsuario;
            usuarioActual.clave = usuarioModificar.clave;
            usuarioActual.nombre = usuarioModificar.nombre;
            usuarioActual.apellido = usuarioModificar.apellido;

            _blogDBContext.Entry(usuarioActual).State = EntityState.Modified;
            _blogDBContext.SaveChanges();

            return Ok(usuarioModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = (from u in _blogDBContext.usuarios
                         where u.usuarioId == id
                         select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            _blogDBContext.usuarios.Attach(usuario);
            _blogDBContext.usuarios.Remove(usuario);
            _blogDBContext.SaveChanges();

            return Ok(usuario);
        }

        [HttpGet]
        [Route("GetUsuariosFiltrado")]
        public IActionResult GetUsuariosFiltrados(string nombre = null, string apellido= null)
        {
            var usuario = from u in _blogDBContext.usuarios
                           select u;

            if (!string.IsNullOrEmpty(nombre)) 
            {
                usuario = usuario.Where(u => u.nombre.Contains(nombre));
            }
            if (!string.IsNullOrEmpty(apellido))
            {
                usuario = usuario.Where(u => u.apellido.Contains(apellido));
            }

            var usuariofiltrado = usuario.ToList();

            if (usuariofiltrado.Count == 0)
            { 
                return NotFound("No se encontraron usuarios con los filtros proporcionados.");
            }

            return Ok(usuariofiltrado);
        }

        [HttpGet]
        [Route("BuscarUsuarioPorRol")]
        public IActionResult BuscarUsuarioPorRol(string rol)
        {
            var usuarios = (from u in _blogDBContext.usuarios
                            join r in _blogDBContext.roles on u.usuarioId equals r.rolId
                            where r.rol == rol
                            select new 
                            {
                                u.usuarioId,
                                u.nombreUsuario,
                                r.rol
                            }).ToList();

            if (usuarios.Count == 0)
            {
                return NotFound($"No se encontraron libros con el título '{rol}'.");
            }

            return Ok(usuarios);
        }
    }
}
