namespace Teams.Models;

public partial class Target
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTimeOffset Created { get; set; }  = DateTime.Now.ToUniversalTime();
    public DateTimeOffset? Deadline { get; set; }
    public int Groupid { get; set; }
    public string? Report { get; set; }
    public int? Status { get; set; }
    public virtual Group Group { get; set; } = null!;
    public virtual ICollection<Executor> Executors { get; set; }
}
