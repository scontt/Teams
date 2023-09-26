using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teams.Data;
using Teams.DTO;
using Teams.Interfaces;

namespace Teams.Repository
{
    [Authorize]
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _context;

        public GroupRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Group> GetAllGroups()
        {
            return _context.Groups.OrderBy(g => g.Id).ToList();
        }

        public async Task<Group> GetGroup(int groupId)
        {
            return _context.Groups.Where(g => g.Id == groupId).AsNoTracking().FirstOrDefault();
        }

        public Member GetMember(int groupId)
        {
            return _context.Members.Where(m => m.GroupId == groupId).FirstOrDefault();
        }

        public ICollection<Member> GetMembers(int groupId)
        {
            return _context.Members.OrderBy(m => m.UserId).Where(m => m.GroupId == groupId).ToList();
        }

        public bool CreateGroup(Group group)
        {
            _context.Add(group);
            return Save();
        }

        public bool IsGroupExists(int groupId)
        {
            return _context.Groups.Any(g => g.Id == groupId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateGroup(Group group)
        {
            _context.Update(group);
            return Save();
        }

        public bool AddNewMember(Member newMember)
        {
            _context.Add(newMember);
            return Save();
        }

        public bool DeleteMember(Member memberDelete)
        {
            _context.Remove(memberDelete);
            return Save();
        }

        public ICollection<Group> GetUsersGroups(int userId)
        {
            return _context.Groups.Where(g => g.OwnerId == userId).OrderBy(g => g.Id).ToList();
        }

        public async Task<bool> DeleteGroup(int groupId)
        {
            var group = await GetGroup(groupId);
            _context.Remove(group);
            return Save();
        }
    }
}