using DAL.Context;
using DAL.DAO;

namespace DAL.Repositories
{
    public class CategoryDetailsRepository
    {
        private readonly db_userservicesContext _userservicesContext;

        public CategoryDetailsRepository(db_userservicesContext db_UserservicesContext)
        {
            _userservicesContext = db_UserservicesContext;
        }

        public List<Categorydetail> GetCategorydetailByUserId(string id)
        {
            return _userservicesContext.Categorydetails.Where(c => c.UserId == id).ToList();
        }

        public string SaveCategory(string userId, List<CateDAO> cateDAO)
        {
            var category = GetCategorydetailByUserId(userId);
            var saveItemToCate = new Categorydetail();
            try
            {
                foreach (var cate in cateDAO)
                {
                    foreach (var item in category)
                    {
                        if (item.ItemId.Equals(cate.itemId))
                        {
                            saveItemToCate.UserId = userId;
                            saveItemToCate.ItemId = item.ItemId;
                            saveItemToCate.Quantity = cate.quantity;
                            _userservicesContext.Categorydetails.Update(saveItemToCate);
                        }
                    }
                    saveItemToCate.UserId = userId;
                    saveItemToCate.ItemId = cate.itemId;
                    saveItemToCate.Quantity = cate.quantity;
                    _userservicesContext.Categorydetails.Update(saveItemToCate);
                }
                _userservicesContext.SaveChanges();
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
