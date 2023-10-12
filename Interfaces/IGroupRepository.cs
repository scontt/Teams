namespace Teams.Interfaces
{
    public interface IGroupRepository
    {
        Task<ICollection<Group>> GetAllGroups();
        Task<Group> GetGroup(int groupId);
        ICollection<Member> GetMembers(int groupId);
        Member GetMember(int groupId);
        Task<bool> CreateGroup(Group group);
        Task<bool> UpdateGroup(Group updatedGroup);
        Task<bool> AddNewMember(Member newMember);
        Task<bool> IsGroupExists(int groupId);
        Task<bool> DeleteMember(Member memberDelete);
        Task<bool> Save();
        ICollection<Group> GetUsersGroups(int id);
        Task<bool> DeleteGroup(int groupId);
    }
}