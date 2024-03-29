using System.Collections.Generic;
using apiAspNet.Models;

namespace apiAspNet.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAllUsers();
        UserDto GetUserById(int id);
        void CreateUser(CreateUserDto userDto);
        void UpdateUser(int id, UpdateUserDto userDto);
        void DeleteUser(int id);
    }
}
