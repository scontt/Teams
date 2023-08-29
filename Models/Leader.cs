namespace Teams.Models
{
    public class Leader
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public User User { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}