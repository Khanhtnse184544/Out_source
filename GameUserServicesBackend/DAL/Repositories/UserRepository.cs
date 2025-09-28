using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository
    {
        private readonly db_userservicesContext _userservicesContext;

        public UserRepository(db_userservicesContext db_UserservicesContext)
        {
            _userservicesContext = db_UserservicesContext;
        }

        public async Task<List<User>> GetAllUserAsync(CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                return await _userservicesContext.Users.AsNoTracking().ToListAsync(cancellationToken);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[UserRepository] GetAllUserAsync took {sw.ElapsedMilliseconds}ms");
            }
        }

        public async Task<User?> GetUserByEmailPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                return await _userservicesContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email && u.Password == password, cancellationToken);
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[UserRepository] GetUserByEmailPasswordAsync took {sw.ElapsedMilliseconds}ms");
            }
        }

        public async Task<string> AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                string id = Guid.NewGuid().ToString();
                user.UserId = id;
                user.Level = 0;
                user.Coin = 10;
                user.MemberTypeId = "M1";
                user.Status = "active";
                _userservicesContext.Users.Add(user);
                await _userservicesContext.SaveChangesAsync(cancellationToken);
                return "RegisterSuccess";
            }
            catch (Exception e)
            {
                Console.WriteLine($"[UserRepository] AddUserAsync failed: {e.Message}");
                return $"{e}";
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($"[UserRepository] AddUserAsync took {sw.ElapsedMilliseconds}ms");
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
