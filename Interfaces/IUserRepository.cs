using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUsername(string username);
        Task<ICollection<User>> GetAllUsers();
        Task<bool> CreateUser(User newUser);
        Task<bool> Save();
    }
}