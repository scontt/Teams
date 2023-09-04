using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.DTO
{
    public class GroupDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int OwnerId { get; set; }
    }
}