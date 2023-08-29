using Teams.Data;
using Teams.Interfaces;

namespace Teams.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Group>> GetAllGroups()
        {
            return _context.Groups.OrderBy(g => g.Id).ToList();
        }

        public async Task<Group> GetGroup(int groupId)
        {
            return _context.Groups.Where(g => g.Id == groupId).FirstOrDefault();
        }

        public async Task<Member> GetMember(int groupId)
        {
            return _context.Members.Where(m => m.GroupId == groupId).FirstOrDefault();
        }

        public async Task<ICollection<Member>> GetMembers(int groupId)
        {
            return _context.Members.OrderBy(m => m.UserId).Where(m => m.GroupId == groupId).ToList();
        }

        public async Task<bool> IsGroupExists(int groupId)
        {
            return _context.Groups.Any(g => g.Id == groupId);
        }
    }
}