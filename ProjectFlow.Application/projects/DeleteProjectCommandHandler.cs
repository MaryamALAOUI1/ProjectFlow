using MediatR;
using Microsoft.EntityFrameworkCore; 
using ProjectFlow.Domain.Events;   
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Projects;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly ProjectDbContext _context;

    public DeleteProjectCommandHandler(ProjectDbContext context) => _context = context;

    public async Task Handle(DeleteProjectCommand request, CancellationToken token)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, token);

        if (project == null)
        {
            return;
        }

        
        project.AddDomainEvent(new ProjectDeletedEvent(project));

        _context.Projects.Remove(project);

       
        await _context.SaveChangesAsync(token);
    }
}