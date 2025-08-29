using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class APIController : ControllerBase
    {
        protected string userName => (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;
        protected List<string> roleList => (HttpContext.User.Identity as ClaimsIdentity).FindAll(ClaimTypes.Role).Select(x=>x.Value).ToList();
    }
}