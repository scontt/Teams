using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.Interfaces
{
    public interface IGroupRepository
    {
        Task<ICollection<Group>> GetAllGroups();
        Task<Group> GetGroup(int groupId);
        Task<ICollection<Member>> GetMembers(int groupId);
        Task<Member> GetMember(int groupId);
        Task<bool> IsGroupExists(int groupId);
    }
}