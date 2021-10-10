using Microsoft.AspNetCore.Mvc;

namespace Exchange.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "Exchange is running ..";
        }
    }
}
