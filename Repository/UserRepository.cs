using Teams.Data;
using Teams.Interfaces;

namespace Teams.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateUser(User newUser)
        {
            _context.Add(newUser);

            return Save();
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(u => u.Id).ToList();
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}