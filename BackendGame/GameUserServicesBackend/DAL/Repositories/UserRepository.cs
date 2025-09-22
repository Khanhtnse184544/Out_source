using DAL.Context;

namespace DAL.Repositories
{
    public class UserRepository
    {
        private readonly db_userservicesContext _userservicesContext;

        public UserRepository(db_userservicesContext db_UserservicesContext)
        {
            _userservicesContext = db_UserservicesContext;
        }

        public List<User> GetAllUser()
        {
            return _userservicesContext.Users.ToList();
        }

        public User? GetUserByEmailPassword(string email, string password)
        {
            return _userservicesContext.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
        }

        public string AddUser(User user)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                user.UserId = id;
                user.Level = 0;
                user.Coin = 10;
                user.MemberTypeId = "M1";
                user.Status = "active";
                _userservicesContext.Users.Add(user);
                _userservicesContext.SaveChanges();
                return "RegisterSuccess";
            }
            catch (Exception e)
            {
                return $"{e}";
            }
        }

        //public string CreateUser(UserDAO newUser)
        //{
        //    var listUser = GetAllUser();
        //    newUser.userId = "U" + new Random().Next(0, 999);
        //    foreach (var user in listUser) 
        //    {
        //        if (user.UserId.Equals(newUser.userId))
        //        {
        //            newUser.userId = "U" + new Random().Next(0, 999);
        //        }
        //    }
        //    return null;
        //}

    }
}
