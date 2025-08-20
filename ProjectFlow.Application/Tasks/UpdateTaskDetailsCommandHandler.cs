using MediatR;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Tasks;

public class UpdateTaskDetailsCommandHandler : IRequestHandler<UpdateTaskDetailsCommand>
{
    private readonly ProjectDbContext _context;
    public UpdateTaskDetailsCommandHandler(ProjectDbContext context) => _context = context;

    public async Task Handle(UpdateTaskDetailsCommand request, CancellationToken token)
    {
        var task = await _context.TaskItems.FindAsync(request.TaskId);
        if (task == null) return; 

        task.Status = request.Status;
        task.DueDate = request.DueDate;

        await _context.SaveChangesAsync(token);
    }
}