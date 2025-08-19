using MediatR;
using ProjectFlow.Domain;

namespace ProjectFlow.Application.Projects;

public class CreateProjectCommand : IRequest<Project>
{
    public string Name { get; set; } = string.Empty;
}