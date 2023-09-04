namespace Teams.Interfaces
{
    public interface ITargetRepository
    {
        Target GetTarget(int targetId);
        ICollection<Target> GetTargets();
        bool CreateTarget(Target target);
        bool UpdateTarget(Target target);
        bool addNewExecutor(Executor executorAdd);
        int GetLastTarget();
        bool DeleteExecutor(Executor executor);
        bool Save();
    }
}