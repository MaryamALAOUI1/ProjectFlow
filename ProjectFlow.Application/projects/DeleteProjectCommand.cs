using MediatR;
namespace ProjectFlow.Application.Projects;
public class DeleteProjectCommand : IRequest
{
    public int Id { get; set; }
}