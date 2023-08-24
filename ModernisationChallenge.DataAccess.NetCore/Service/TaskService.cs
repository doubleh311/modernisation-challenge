namespace ModernisationChallenge.DataAccess.NetCore
{
    public class TaskService : ITaskService
    {
        private readonly DataContext _dataContext;

        public TaskService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task AddTask(Task task)
        {
            _dataContext.Tasks.Add(task);
            _dataContext.SaveChanges();
            return task;
        }

        public Task CompleteTask(int taskId)
        {
            var task = GetTaskById(taskId);
            task.Completed = !task.Completed;
            _dataContext.SaveChanges();
            return task;
        }

        public Task GetTaskById(int taskId)
        {
            if (_dataContext.Tasks.Any(x => x.Id == taskId))
            {
                return _dataContext.Tasks.First(x => x.Id == taskId);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public IEnumerable<Task> GetTasks()
        {
            return _dataContext.Tasks.Where(x => x.DateDeleted == null).AsEnumerable();
        }

        public Task RemoveTaskById(int taskId)
        {
            var taskToUpdate = GetTaskById(taskId);
            taskToUpdate.DateDeleted = DateTime.Now;
            _dataContext.SaveChanges();
            return taskToUpdate;
        }

        public Task UpdateTask(Task task)
        {
            var taskToUpdate = GetTaskById(task.Id.Value);
            taskToUpdate.Details = task.Details;
            _dataContext.SaveChanges();
            return taskToUpdate;
        }
    }
}