using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain.Events;

public class ProjectUpdatedEvent : BaseEvent
{
    public Project Project { get; }
    public ProjectUpdatedEvent(Project project) => Project = project;
}