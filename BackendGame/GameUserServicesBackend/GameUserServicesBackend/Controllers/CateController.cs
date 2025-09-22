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
        public IActionResult LoadCategoryByUSer(string userId)
        {
            return Ok(_categoryDetailServices.GetCategorydetailByUserId(userId));
        }

        [HttpPut("SaveCategory")]
        public IActionResult SaveCategory(string userId, List<CateDAO> cateDAO)
        {
            return Ok(_categoryDetailServices.AddCategorydetail(userId, cateDAO));
        }
    }
}
