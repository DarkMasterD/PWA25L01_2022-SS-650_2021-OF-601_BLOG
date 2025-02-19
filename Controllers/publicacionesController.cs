using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_NUMEROS_CARNETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class publicacionesController : ControllerBase
    {
        private readonly blogDBContext _blogDBContext;

        public publicacionesController(blogDBContext publicacionesContexto)
        {
            _blogDBContext = publicacionesContexto;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<publicaciones> listadoPublicaciones = (from p in _blogDBContext.publicaciones
                                              select p).ToList();

            if (listadoPublicaciones.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoPublicaciones);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPublicacion([FromBody] publicaciones publicacion)
        {
            try
            {
                _blogDBContext.publicaciones.Add(publicacion);
                _blogDBContext.SaveChanges();
                return Ok(publicacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPublicacion(int id, [FromBody] publicaciones publicacionModificar)
        {
            var publicacionActual = (from p in _blogDBContext.publicaciones
                                 where p.publicacionId == id
                                 select p).FirstOrDefault();

            if (publicacionActual == null)
            {
                return NotFound();
            }

            publicacionActual.titulo = publicacionModificar.titulo;
            publicacionActual.descripcion = publicacionModificar.descripcion;
            publicacionActual.usuarioId = publicacionModificar.usuarioId;

            _blogDBContext.Entry(publicacionActual).State = EntityState.Modified;
            _blogDBContext.SaveChanges();

            return Ok(publicacionModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPublicacion(int id)
        {
            var publicacion = (from p in _blogDBContext.publicaciones
                           where p.publicacionId == id
                           select p).FirstOrDefault();

            if (publicacion == null)
            {
                return NotFound();
            }

            _blogDBContext.publicaciones.Attach(publicacion);
            _blogDBContext.publicaciones.Remove(publicacion);
            _blogDBContext.SaveChanges();

            return Ok(publicacion);
        }

        [HttpGet]
        [Route("PublicacionesFiltradasPorUsuario")]
        public IActionResult PublicacionesFiltradasPorUsuario(string usuario)
        {
            var publicaciones = (from p in _blogDBContext.publicaciones
                            join u in _blogDBContext.usuarios on p.usuarioId equals u.usuarioId
                            where u.nombreUsuario == usuario
                            select new
                            {
                                u.usuarioId,
                                u.nombreUsuario,
                                p.titulo
                            }).ToList();

            if (!publicaciones.Any())
            {
                return NotFound($"El usuario '{usuario}' no tiene publicaciones.");
            }

            return Ok(publicaciones);
        }

    }
}
