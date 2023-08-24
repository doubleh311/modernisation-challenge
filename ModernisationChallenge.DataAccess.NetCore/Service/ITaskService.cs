namespace ModernisationChallenge.DataAccess.NetCore
{
    public interface ITaskService
    {
        Task AddTask(Task task);
        Task UpdateTask(Task task);
        Task CompleteTask(int taskId);
        Task RemoveTaskById(int taskId);
        IEnumerable<Task> GetTasks();
        Task GetTaskById(int taskId);
    }
}