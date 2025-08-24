using MediatR;
using Microsoft.Extensions.Logging;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Application.projects.EventHandlers;

public class ProjectDeletedEventHandler : INotificationHandler<ProjectDeletedEvent>
{
    private readonly ILogger<ProjectDeletedEventHandler> _logger;
    public ProjectDeletedEventHandler(ILogger<ProjectDeletedEventHandler> logger) => _logger = logger;

    public Task Handle(ProjectDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DOMAIN EVENT: Project was deleted. Name: \"{ProjectName}\", ID: {ProjectId}",
            notification.Project.Name,
            notification.Project.Id);
        return Task.CompletedTask;
    }
}