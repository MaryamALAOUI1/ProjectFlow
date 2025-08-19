using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Projects;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Project?>
{
    private readonly ProjectDbContext _context;

    public GetProjectByIdQueryHandler(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        return project;
    }
}