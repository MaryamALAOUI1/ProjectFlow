using MediatR;
using Microsoft.EntityFrameworkCore;  
using ProjectFlow.Domain.Events;   
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Tasks;

public class UpdateTaskDetailsCommandHandler : IRequestHandler<UpdateTaskDetailsCommand>
{
    private readonly ProjectDbContext _context;

    public UpdateTaskDetailsCommandHandler(ProjectDbContext context) => _context = context;

    public async Task Handle(UpdateTaskDetailsCommand request, CancellationToken token)
    {
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == request.TaskId, token);

        if (task == null)
        {
            return;
        }

        task.Status = request.Status;
        task.DueDate = request.DueDate;

       
        task.AddDomainEvent(new TaskUpdatedEvent(task));

        await _context.SaveChangesAsync(token);
    }
}