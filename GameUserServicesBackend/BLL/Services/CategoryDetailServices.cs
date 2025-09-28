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
        public async Task<List<Categorydetail>> GetCategorydetailByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _categoryDetailsRepository.GetCategorydetailByUserIdAsync(userId, cancellationToken);
        }

        public async Task<string> AddCategorydetailAsync(string userId, List<CateDAO> cateDAO, CancellationToken cancellationToken = default)
        {
            return await _categoryDetailsRepository.SaveCategoryAsync(userId, cateDAO, cancellationToken);
        }
    }
}
