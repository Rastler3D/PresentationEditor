using Microsoft.AspNetCore.Mvc;
using UserManagementService.Server.Utils;
using PresentationEditor.Server.ViewModel;

namespace UserManagementService.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JwtTokenGenerator jwtGenerator;

        public UserController(JwtTokenGenerator jwtTokenGenerator)
        {
            jwtGenerator = jwtTokenGenerator;
        }


        [HttpPost("Login")]
        public ActionResult<LoginResultModel> Login([FromQuery] string userName)
        {
            return Ok(new LoginResultModel { Token = jwtGenerator.GenerateToken(userName) });
        }
    }  
}