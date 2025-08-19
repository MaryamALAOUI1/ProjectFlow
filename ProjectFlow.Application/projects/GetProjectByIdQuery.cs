using MediatR;
using ProjectFlow.Domain;

namespace ProjectFlow.Application.Projects;

public class GetProjectByIdQuery : IRequest<Project?>
{
    public int Id { get; set; }
}