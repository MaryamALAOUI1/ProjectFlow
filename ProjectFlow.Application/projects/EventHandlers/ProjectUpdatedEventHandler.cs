using MediatR;
using Microsoft.Extensions.Logging;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Application.projects.EventHandlers;

public class ProjectUpdatedEventHandler : INotificationHandler<ProjectUpdatedEvent>
{
    private readonly ILogger<ProjectUpdatedEventHandler> _logger;
    public ProjectUpdatedEventHandler(ILogger<ProjectUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(ProjectUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DOMAIN EVENT: Project was updated. Name: \"{ProjectName}\", ID: {ProjectId}",
            notification.Project.Name,
            notification.Project.Id);
        return Task.CompletedTask;
    }
}