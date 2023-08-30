using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int userId);
        User GetUserByUsername(string username);
        ICollection<User> GetAllUsers();
        bool CreateUser(User newUser);
        bool Save();
    }
}