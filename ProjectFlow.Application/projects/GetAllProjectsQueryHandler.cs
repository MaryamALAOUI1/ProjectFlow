using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;
namespace ProjectFlow.Application.Projects;
public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<Project>>
{
    private readonly ProjectDbContext _context;
    public GetAllProjectsQueryHandler(ProjectDbContext context) => _context = context;

    public async Task<List<Project>> Handle(GetAllProjectsQuery request, CancellationToken token)
    {
        return await _context.Projects.Include(p => p.Tasks).ToListAsync(token);
    }
}