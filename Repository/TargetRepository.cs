using Teams.Data;
using Teams.Interfaces;

namespace Teams.Repository
{
    public class TargetRepository : ITargetRepository
    {
        private readonly DataContext _context;

        public TargetRepository(DataContext context)
        {
            _context = context;
        }

        public bool addNewExecutor(Executor executorAdd)
        {
            _context.Add(executorAdd);
            return Save();
        }

        public bool CreateTarget(Target targetCreate)
        {
            _context.Add(targetCreate);
            return Save();
        }

        public Target GetTarget(int targetId)
        {
            return _context.Targets.Where(t => t.Id == targetId).FirstOrDefault();
        }

        public ICollection<Target> GetTargets()
        {
            return _context.Targets.OrderBy(t => t.Id).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateTarget(Target targetUpdate)
        {
            throw new NotImplementedException();
        }

        public int GetLastTarget()
        {
            return _context.Targets.Max(t => t.Id);
        }

        
    }
}