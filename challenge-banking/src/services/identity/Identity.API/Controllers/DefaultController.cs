using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Identity is running ..";
        }
    }
}
