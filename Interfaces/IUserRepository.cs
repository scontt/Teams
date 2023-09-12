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
        bool UpdateUser(User userUpdate);
        User LogIn(User user);
        User SignIn(User user);
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword);
        bool Save();
    }
}