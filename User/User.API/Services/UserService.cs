using User.Domain.Data.Interfaces;

namespace User.Api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    }
}