using BLL.Services;
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
            var user = await _userServices.LoginWithEmailPasswordAsync(payload.Email, payload.Password, cancellationToken);
            if (user == null)
            {
                return Unauthorized(new { status = "error", message = "Invalid credentials" });
            }
            return Ok(user);
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

        [HttpGet("GetUserTool")]
        public IActionResult GetTools(string userId)
        {
            var userTools = _userServices.GetUsertools(userId);
            if (userTools != null)
            {
                return Ok(userTools);
            }
            return BadRequest("Not found");
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserDAO user)
        {
            return Ok(_userServices.UpdateUser(user));
        }

        [HttpPut("SavePlantedLog")]
        public IActionResult SavePlantedLog(string userId, List<string> ItemsId)
        {
            return Ok(_userServices.SavePlantedLog(userId, ItemsId));
        }

        [HttpPut("SaveUserTools")]
        public IActionResult SaveUserTools(string userId, int qtyFr, int qtyPs)
        {
            return Ok(_userServices.SaveUserTools(userId, qtyFr, qtyPs));
        }

        [HttpGet("GetPlantLogByUserId")]
        public IActionResult GetPlantLogByUserId(string userId)
        {
            return Ok(_userServices.GetPlantedlogs(userId));
        }

        [HttpPut("ConvertedTree")]
        public IActionResult ConvertedTree(string userId, int qty, string itemId)
        {
            return Ok(_userServices.ConvertedTree(userId, qty, itemId));
        }

        [HttpGet("GetUserById")]
        public IActionResult GetUserById(string userId)
        {
            return Ok(_userServices.GetUser(userId));
        }
    }
}
