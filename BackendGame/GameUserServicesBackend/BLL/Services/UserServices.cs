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

        public User? LoginWithEmailPassword(string email, string password)
        {
            return _userRepository.GetUserByEmailPassword(email, password);
        }

        public string Register(UserDAO newUser)
        {
            var user = new User();
            user.UserName = newUser.username;
            user.Password = newUser.password;
            user.Email = newUser.email;
            return _userRepository.AddUser(user);
        }
    }
}
