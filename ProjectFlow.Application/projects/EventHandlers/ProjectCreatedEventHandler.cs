using MediatR;
using Microsoft.Extensions.Logging;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Application.projects.EventHandlers;


public class ProjectCreatedEventHandler : INotificationHandler<ProjectCreatedEvent>
{
    private readonly ILogger<ProjectCreatedEventHandler> _logger;

    public ProjectCreatedEventHandler(ILogger<ProjectCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        
        _logger.LogInformation("DOMAIN EVENT: A new project was created with Name: \"{ProjectName}\" and ID: {ProjectId}",
            notification.Project.Name,
            notification.Project.Id);

        return Task.CompletedTask;
    }
}