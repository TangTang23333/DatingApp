using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]  // like router.xxx("api/{controller}", func(req, res){})
    public class BaseApiController : ControllerBase
    {
        


    }
}