using BLL.Services;
using DAL.DAO;
using Microsoft.AspNetCore.Mvc;

namespace GameUserServicesBackend.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CateController : ControllerBase
    {
        private readonly CategoryDetailServices _categoryDetailServices;

        public CateController(CategoryDetailServices categoryDetailServices)
        {
            _categoryDetailServices = categoryDetailServices;
        }
        [HttpGet("CategoryByUserId")]
        public async Task<IActionResult> LoadCategoryByUSer([FromQuery] string userId, CancellationToken cancellationToken)
        {
            var data = await _categoryDetailServices.GetCategorydetailByUserIdAsync(userId, cancellationToken);
            return Ok(data);
        }

        [HttpPut("SaveCategory")]
        public async Task<IActionResult> SaveCategory([FromQuery] string userId, [FromBody] List<CateDAO> cateDAO, CancellationToken cancellationToken)
        {
            var result = await _categoryDetailServices.AddCategorydetailAsync(userId, cateDAO, cancellationToken);
            return Ok(result);
        }
    }
}
