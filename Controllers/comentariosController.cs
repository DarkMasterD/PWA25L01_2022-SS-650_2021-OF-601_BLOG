using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
