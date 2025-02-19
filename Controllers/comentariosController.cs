using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace L01_NUMEROS_CARNETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : ControllerBase
    {
        private readonly blogDBContext _blogDBContext;

        public comentariosController(blogDBContext blogDBContext) 
        {
            _blogDBContext = blogDBContext;        
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Listar()
        {
            List<comentarios> Comentarios = (from c in _blogDBContext.comentarios select c).ToList();

            if (Comentarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(Comentarios);
        }

        [HttpPost]
        [Route("AddComent")]
        public IActionResult Agregar([FromBody] comentarios Comentario)
        {
            try
            {
                _blogDBContext.comentarios.Add(Comentario);
                _blogDBContext.SaveChanges();

                return Ok(Comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateComent/{id}")]
        public IActionResult Modificar(int id, [FromBody] comentarios Comentario)
        {
            try
            {
                comentarios? ComentarioActual = (from c in _blogDBContext.comentarios where c.cometarioId == id select c).FirstOrDefault();

                if (ComentarioActual == null)
                {
                    return NotFound();
                }

                ComentarioActual.publicacionId = Comentario.publicacionId;
                ComentarioActual.comentario = Comentario.comentario;
                ComentarioActual.usuarioId = Comentario.usuarioId;

                _blogDBContext.Entry(ComentarioActual).State = EntityState.Modified;
                _blogDBContext.SaveChanges();

                return Ok(ComentarioActual);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteComent/{id}")]
        public IActionResult Eliminar(int id) 
        {
            comentarios? Comentario = (from c in _blogDBContext.comentarios where c.cometarioId == id select c).FirstOrDefault();

            if(Comentario == null)
            {
                return NotFound();
            }

            _blogDBContext.comentarios.Attach(Comentario);
            _blogDBContext.comentarios.Remove(Comentario);
            _blogDBContext.SaveChanges();

            return Ok(Comentario);
        }

        [HttpGet]
        [Route("SearchComentsByPost/{id}")]
        public IActionResult ComentariosPublicacion(int id)
        {
            var ComentariosPubli = (from p in _blogDBContext.publicaciones
                                    where p.publicacionId == id
                                    select new
                                    {
                                        p.titulo,
                                        p.descripcion,
                                        comentarios = (from c in _blogDBContext.comentarios where c.publicacionId == id select new
                                        {
                                            UsuarioId = c.usuarioId,
                                            Id = c.cometarioId,
                                            c.comentario
                                        }).ToList()
                                    }).FirstOrDefault();

            if (ComentariosPubli == null)
            {
                return NotFound();
            }

            return Ok(ComentariosPubli);
        }

        [HttpGet]
        [Route("GetTopPostComents/{cantidad}")]
        public IActionResult PublicacionMasComentarios(int cantidad)
        {
            var Comentarios = (from c in _blogDBContext.comentarios
                               join p in _blogDBContext.publicaciones
                               on c.publicacionId equals p.publicacionId
                               select new
                               {
                                   p.titulo
                               }).GroupBy(x => x.titulo).Select(c => new
                               {
                                   Publicacion = c.Key,
                                   Cantidad = c.Count()
                               }).OrderByDescending(x => x.Cantidad).Take(cantidad).ToList();

            if(Comentarios == null)
            {
                return NotFound();
            }

            return Ok(Comentarios);
        }
    }
}
