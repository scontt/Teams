using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.Interfaces
{
    public interface IUserRepository
    {
        public User GetUser(int userId);
    }
}