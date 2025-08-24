using MediatR;
using Microsoft.Extensions.Logging;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Application.Tasks.EventHandlers;

public class TaskCompletedEventHandler : INotificationHandler<TaskCompletedEvent>
{
    private readonly ILogger<TaskCompletedEventHandler> _logger;

    public TaskCompletedEventHandler(ILogger<TaskCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TaskCompletedEvent notification, CancellationToken cancellationToken)
    {
       
        _logger.LogInformation("Domain Event Handled: Task '{TaskTitle}' ({TaskId}) was completed.",
            notification.TaskItem.Title,
            notification.TaskItem.Id);

        return Task.CompletedTask;
    }
}