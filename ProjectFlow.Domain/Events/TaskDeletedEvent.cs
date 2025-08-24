using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain.Events;

public class TaskDeletedEvent : BaseEvent
{
    public TaskItem Task { get; }
    public TaskDeletedEvent(TaskItem task) => Task = task;
}