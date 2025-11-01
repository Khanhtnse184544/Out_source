using DAL.Context;
using DAL.DAO;
using DAL.Repositories;

namespace BLL.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepository;
        public UserServices(UserRepository userRepository, UserDAO userDAO)
        {
            _userRepository = userRepository;

        }

        public async Task<User?> LoginWithEmailPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetUserByEmailPasswordAsync(email, password, cancellationToken);
        }

        public async Task<string> RegisterAsync(UserDAO newUser, CancellationToken cancellationToken = default)
        {
            var user = new User();
            user.UserName = newUser.Username;
            user.Password = newUser.Password;
            user.Email = newUser.Email;
            return await _userRepository.AddUserAsync(user, cancellationToken);
        }

        public List<Usertool> GetUsertools(string userId)
        {
            return _userRepository.GetUsertoolByUserId(userId);
        }

        public string UpdateUser(UserDAO user)
        {
            if (user != null)
            {
                _userRepository.UpdateUser(user.UserId, user.Level, user.Coin, user.ExpPerLevel);
                return "Success";
            }
            return "fails";
        }

        public string SavePlantedLog(string userId, List<string> ItemsId)
        {
            return _userRepository.SavePlantedLog(userId, ItemsId);
        }

        public string SaveUserTools(string userId, int qtyFr, int qtyPs)
        {
            return _userRepository.SaveUserTools(userId, qtyFr, qtyPs);
        }

        public List<Plantedlog> GetPlantedlogs(string userId)
        {
            return _userRepository.GetPlantedlogsByUserId(userId);
        }

        public List<Plantedlog> ConvertedTree(string userId, int qty, string itemId)
        {
            return _userRepository.ConvertTree(userId, qty, itemId);
        }

        public User? GetUser(string userId)
        {
            return _userRepository.GetUserById(userId);
        }
    }
}
