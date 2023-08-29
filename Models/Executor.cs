using Teams.Models;

namespace Teams.Models
{
    public class Executor
    {
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public User User { get; set; } = null!;
        public Target Target { get; set; } = null!;
    }
}