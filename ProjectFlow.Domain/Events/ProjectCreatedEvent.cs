using ProjectFlow.Domain.Common;
using ProjectFlow.Domain; 
namespace ProjectFlow.Domain.Events;


public class ProjectCreatedEvent : BaseEvent
{
   
    public Project Project { get; }

    public ProjectCreatedEvent(Project project)
    {
        Project = project;
    }
}