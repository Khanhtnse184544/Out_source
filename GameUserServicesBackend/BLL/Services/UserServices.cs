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
            user.UserName = newUser.username;
            user.Password = newUser.password;
            user.Email = newUser.email;
            return await _userRepository.AddUserAsync(user, cancellationToken);
        }
    }
}
