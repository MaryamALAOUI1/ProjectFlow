using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Infrastructure;
namespace ProjectFlow.Application.Projects;
public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly ProjectDbContext _context;
    public DeleteProjectCommandHandler(ProjectDbContext context) => _context = context;
    public async Task Handle(DeleteProjectCommand request, CancellationToken token)
    {
        var project = await _context.Projects.FindAsync(request.Id);
        if (project == null) return;
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(token);
    }
}