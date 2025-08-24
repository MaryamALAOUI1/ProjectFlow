using MediatR;
using Microsoft.Extensions.Logging;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Application.Tasks.EventHandlers;

public class TaskCreatedEventHandler : INotificationHandler<TaskCreatedEvent>
{
    private readonly ILogger<TaskCreatedEventHandler> _logger;
    public TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(TaskCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DOMAIN EVENT: A new task was created. Title: \"{TaskTitle}\", ID: {TaskId}, ProjectID: {ProjectId}",
            notification.Task.Title,
            notification.Task.Id,
            notification.Task.ProjectId);
        return Task.CompletedTask;
    }
}