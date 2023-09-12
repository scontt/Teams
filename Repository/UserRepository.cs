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

        public User LogIn(User user)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public User SignIn(User user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User userUpdate)
        {
            _context.Update(userUpdate);
            return Save();
        }
        
        public string HashPassword(string password)
        {           
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword))
                return true;
            return false;
        }
    }
}