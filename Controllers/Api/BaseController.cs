using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace LibApp.Controllers.Api
{

    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {

    } 
}