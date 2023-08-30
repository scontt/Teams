namespace Teams.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string Password { get; set; } = null!;
        public int? Donetasks { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Executor> Executors { get; set; }
        public virtual ICollection<Group> OwnsGroups { get; set; }
    }
}