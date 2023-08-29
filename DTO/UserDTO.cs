using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string Password { get; set; } = null!;
        public int? Donetasks { get; set; }
    }
}