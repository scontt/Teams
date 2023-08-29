using System;
using System.Collections.Generic;

namespace Teams.Models;

public partial class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public virtual ICollection<Target> Targets { get; set; }
    public virtual ICollection<Leader> Leaders { get; set; }
    public virtual ICollection<Member> Members { get; set; }
}
