using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain.Events;

public class ProjectDeletedEvent : BaseEvent
{
    public Project Project { get; }
    public ProjectDeletedEvent(Project project) => Project = project;
}