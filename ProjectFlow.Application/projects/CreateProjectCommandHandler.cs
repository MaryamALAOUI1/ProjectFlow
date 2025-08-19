using MediatR;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Projects;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly ProjectDbContext _context;

    public CreateProjectCommandHandler(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Name,
            CreationDate = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return project;
    }
}