using DAL.Context;
using DAL.DAO;
using DAL.Repositories;

namespace BLL.Services
{
    public class CategoryDetailServices
    {
        private readonly CategoryDetailsRepository _categoryDetailsRepository;

        public CategoryDetailServices(CategoryDetailsRepository categoryDetailsRepository)
        {
            _categoryDetailsRepository = categoryDetailsRepository;
        }
        public List<Categorydetail> GetCategorydetailByUserId(string userId)
        {
            return _categoryDetailsRepository.GetCategorydetailByUserId(userId);
        }

        public string AddCategorydetail(string userId, List<CateDAO> cateDAO)
        {
            return _categoryDetailsRepository.SaveCategory(userId, cateDAO);
        }
    }
}
