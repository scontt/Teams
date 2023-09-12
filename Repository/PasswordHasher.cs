using Teams.Interfaces;

namespace Teams.Repository
{
    public class PasswordHasher : IHasher
    {
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