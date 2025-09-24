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
        public User? Login(string email, string password)
        {
            return _userServices.LoginWithEmailPassword(email, password);
        }

        [HttpPut("Register")]
        public IActionResult Register([FromBody] UserDAO user)
        {
            _userServices.Register(user);
            return BadRequest();
        }
    }
}
