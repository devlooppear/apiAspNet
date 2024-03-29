// IUserRepository.cs
using apiAspNet.Models;

namespace apiAspNet.Repositories
{
    public interface IUserRepository
    {
        void CreateUser(User user);
    }
}