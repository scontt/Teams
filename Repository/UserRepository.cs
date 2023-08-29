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

        public async Task<bool> CreateUser(User newUser)
        {
            _context.Add(newUser);

            return await Save();
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            return _context.Users.OrderBy(u => u.Id).ToList();
        }

        public async Task<User> GetUserById(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public async Task<bool> Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}