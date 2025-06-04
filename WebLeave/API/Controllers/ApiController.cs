using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ApiController : ControllerBase
    {
        protected string UserId => (HttpContext.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}