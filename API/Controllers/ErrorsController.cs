using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class ErrorsController : ApiController
    {
        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
