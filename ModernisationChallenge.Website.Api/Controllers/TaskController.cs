using Microsoft.AspNetCore.Mvc;
using ModernisationChallenge.DataAccess.NetCore;
using ModernisationChallenge.Website.Api.Model;

namespace ModernisationChallenge.Website.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoTask>> Get()
        {
            return Ok(_taskService.GetTasks());
        }

        [HttpGet("id")]
        public ActionResult<TodoTask> GetTask(int id)
        {
            return Ok(_taskService.GetTaskById(id));
        }

        [HttpPost]
        public ActionResult<TodoTask> AddTask(TodoTask task)
        {
            return Ok(_taskService.AddTask(new DataAccess.Task 
            {
                Details = task.Details   
            }));
        }

        [HttpPut]
        public ActionResult<TodoTask> UpdateTask(TodoTask task)
        {
            return Ok(_taskService.UpdateTask(new DataAccess.Task
            {
                Id = task.Id,
                Details = task.Details
            }));
        }

        [HttpPatch]
        public ActionResult<TodoTask> CompleteTask(int id)
        {
            return Ok(_taskService.CompleteTask(id));
        }

        [HttpDelete]
        public ActionResult<TodoTask> RemoveTask(int id)
        {
            return Ok(_taskService.RemoveTaskById(id));
        }
    }
}