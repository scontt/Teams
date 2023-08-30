namespace Teams.Interfaces
{
    public interface ITargetRepository
    {
        Target GetTarget(int targetId);
        ICollection<Target> GetTargets();
        bool CreateTarget(Target targetCreate);
        bool UpdateTarget(Target targetUpdate);
        bool addNewExecutor(Executor executorAdd);
        int GetLastTarget();
        bool Save();
    }
}