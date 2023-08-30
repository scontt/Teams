using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.DTO
{
    public class TargetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Deadline { get; set; }
        public int Groupid { get; set; }
        public string? Report { get; set; }
        public int? Status { get; set; }
    }
}