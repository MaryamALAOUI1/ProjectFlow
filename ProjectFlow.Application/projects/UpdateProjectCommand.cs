using MediatR;
namespace ProjectFlow.Application.Projects;
public class UpdateProjectCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}