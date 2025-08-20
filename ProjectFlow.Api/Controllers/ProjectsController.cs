using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectFlow.Application.Projects; 
using ProjectFlow.Domain;

namespace ProjectFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(CreateProjectCommand command)
        {
            var createdProject = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            var query = new GetProjectByIdQuery { Id = id };
            var project = await _mediator.Send(query);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetAllProjects()
        {
            var projects = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(projects);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, UpdateProjectCommand command)
        {
            if (id != command.Id) return BadRequest();
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _mediator.Send(new DeleteProjectCommand { Id = id });
            return NoContent();
        }
    }
}