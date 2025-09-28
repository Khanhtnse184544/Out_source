using BLL.Services;
using DAL.Context;
using DAL.DAO;
using Microsoft.AspNetCore.Mvc;

namespace GameUserServicesBackend.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDAO payload, CancellationToken cancellationToken)
        {
            var user = await _userServices.LoginWithEmailPasswordAsync(payload.email, payload.password, cancellationToken);
            if (user == null)
            {
                return Unauthorized(new { status = "error", message = "Invalid credentials" });
            }
            return Ok(new { status = "success", data = user });
        }

        [HttpPut("Register")]
        public async Task<IActionResult> Register([FromBody] UserDAO user, CancellationToken cancellationToken)
        {
            var result = await _userServices.RegisterAsync(user, cancellationToken);
            if (result == "RegisterSuccess")
            {
                return Ok(new { status = "success", message = result });
            }
            return BadRequest(new { status = "error", message = result });
        }
    }
}
