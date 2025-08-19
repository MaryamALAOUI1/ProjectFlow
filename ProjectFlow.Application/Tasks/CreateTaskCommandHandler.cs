using FluentValidation;
using MediatR;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Tasks;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskItem>
{
    private readonly ProjectDbContext _context;
    private readonly IValidator<CreateTaskCommand> _validator;

    public CreateTaskCommandHandler(ProjectDbContext context, IValidator<CreateTaskCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<TaskItem> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken); 

        var taskItem = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = "To Do", 
            ProjectId = request.ProjectId 
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync(cancellationToken);

        return taskItem;
    }
}