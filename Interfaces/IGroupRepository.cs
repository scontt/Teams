using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Teams.Interfaces
{
    public interface IGroupRepository
    {
        ICollection<Group> GetAllGroups();
        Task<Group> GetGroup(int groupId);
        ICollection<Member> GetMembers(int groupId);
        Member GetMember(int groupId);
        bool CreateGroup(Group group);
        bool UpdateGroup(Group updatedGroup);
        bool AddNewMember(Member newMember);
        bool IsGroupExists(int groupId);
        bool DeleteMember(Member memberDelete);
        bool Save();
        ICollection<Group> GetUsersGroups(int id);
        Task<bool> DeleteGroup(int groupId);
    }
}