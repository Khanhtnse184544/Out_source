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
            var allUser = await GetAllUserAsync(cancellationToken);
            foreach (var item in allUser)
            {
                if (user.UserName == item.UserName)
                {
                    return "User already exits";
                }
                if (user.Email == item.Email)
                {
                    return "Email exited";
                }
            }
            try
            {
                string id = Guid.NewGuid().ToString();
                user.UserId = id;
                user.Level = 0;
                user.Coin = 100;
                user.ExpPerLevel = 0;
                user.MemberTypeId = "M1";
                user.Status = "active";
                _userservicesContext.Users.Add(user);
                _userservicesContext.SaveChanges();
                //=================================
                var userCate1 = new Categorydetail();
                userCate1.UserId = user.UserId;
                userCate1.ItemId = "TA0";
                userCate1.Quantity = 2;
                _userservicesContext.Categorydetails.Add(userCate1);
                _userservicesContext.SaveChanges();
                //=================================
                var userCate2 = new Categorydetail();
                userCate2.UserId = user.UserId;
                userCate2.ItemId = "TKE01";
                userCate2.Quantity = 2;
                _userservicesContext.Categorydetails.Add(userCate2);
                _userservicesContext.SaveChanges();
                //=================================
                _userservicesContext.Usertools.AddRange(new[]
                {
                    new Usertool { UserId = user.UserId, ToolsId = "FR", Quantity = 10 },
                    new Usertool { UserId = user.UserId, ToolsId = "PS", Quantity = 10 }
                });
                //=================================
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

        public List<Usertool> GetUsertoolByUserId(string userId)
        {
            return _userservicesContext.Usertools.Where(u => u.UserId == userId).ToList();
        }

        public string UpdateUser(string userId, int level, int coins, int expPerlevel)
        {
            var userUpdate = _userservicesContext.Users.SingleOrDefault(u => u.UserId == userId);
            if (userUpdate != null)
            {
                userUpdate.Level = level;
                userUpdate.ExpPerLevel = expPerlevel;
                userUpdate.Coin = coins;
                _userservicesContext.Users.Update(userUpdate);
                _userservicesContext.SaveChanges();
                return "success";
            }
            return "fail";
        }

        public string SavePlantedLog(string userId, List<string> ItemIds)
        {
            var listOld = _userservicesContext.Plantedlogs.Where(p => p.UserId == userId && p.Status == "Planted").ToList();
            foreach (var item in listOld)
            {
                _userservicesContext.Plantedlogs.Remove(item);
            }
            if (ItemIds != null)
            {
                foreach (var item in ItemIds)
                {
                    var newPlantedLog = new Plantedlog();
                    newPlantedLog.Id = Guid.NewGuid().ToString();
                    newPlantedLog.UserId = userId;
                    newPlantedLog.ItemId = item;
                    newPlantedLog.Status = "Planted";
                    _userservicesContext.Plantedlogs.Add(newPlantedLog);
                    _userservicesContext.SaveChanges();
                }
                return "Success";
            }
            return "Failed";
        }



        public string SaveUserTools(string userId, int qtyFer, int qtyPes)
        {
            var userToolsList = _userservicesContext.Usertools.Where(u => u.UserId == userId).ToList();
            if (userToolsList != null)
            {
                foreach (var userTool in userToolsList)
                {
                    if (userTool.ToolsId.Equals("FR"))
                    {
                        userTool.Quantity = qtyFer;
                    }
                    else if (userTool.ToolsId.Equals("PS"))
                    {
                        userTool.Quantity = qtyPes;
                    }
                    _userservicesContext.Usertools.Update(userTool);
                    _userservicesContext.SaveChanges();
                }
                return "Success";
            }
            return "Fail";
        }

        public List<Plantedlog> GetPlantedlogsByUserId(string userId)
        {
            var listPlanted = _userservicesContext.Plantedlogs.Where(u => u.UserId == userId && u.Status == "Planted").ToList();
            if (listPlanted != null)
            {
                return listPlanted;
            }
            return null;
        }

        public List<Plantedlog> ConvertTree(string userId, int qtyRequired, string itemId)
        {
            var listConvert = _userservicesContext.Plantedlogs.Where(p => (p.UserId == userId && p.ItemId == itemId) && p.Status == "Planted").Take(qtyRequired).ToList();
            if (listConvert != null)
            {
                foreach (var plantedlog in listConvert)
                {
                    plantedlog.Status = "Converted";
                    _userservicesContext.Plantedlogs.Update(plantedlog);
                }
                _userservicesContext.SaveChanges();
                return listConvert;
            }
            return listConvert;
        }

        public User? GetUserById(string userId)
        {
            return _userservicesContext.Users.FirstOrDefault(u => u.UserId == userId);
        }
    }
}
