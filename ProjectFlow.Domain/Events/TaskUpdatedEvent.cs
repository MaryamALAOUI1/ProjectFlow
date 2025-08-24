using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain.Events;

public class TaskUpdatedEvent : BaseEvent
{
    public TaskItem Task { get; }
    public TaskUpdatedEvent(TaskItem task) => Task = task;
}