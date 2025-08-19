using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectFlow.Application.Tasks;
using ProjectFlow.Domain;

namespace ProjectFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(CreateTaskCommand command)
        {
            var createdTask = await _mediator.Send(command);

            return Ok(createdTask);
        }
    }
}