using MediatR;
using Microsoft.Extensions.Logging;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Application.Tasks.EventHandlers;

public class TaskUpdatedEventHandler : INotificationHandler<TaskUpdatedEvent>
{
    private readonly ILogger<TaskUpdatedEventHandler> _logger;
    public TaskUpdatedEventHandler(ILogger<TaskUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(TaskUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DOMAIN EVENT: Task was updated. Title: \"{TaskTitle}\", ID: {TaskId}, New Status: {Status}",
            notification.Task.Title,
            notification.Task.Id,
            notification.Task.Status);
        return Task.CompletedTask;
    }
}