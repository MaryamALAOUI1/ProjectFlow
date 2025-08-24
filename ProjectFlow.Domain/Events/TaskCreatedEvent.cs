using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain.Events;

public class TaskCreatedEvent : BaseEvent
{
    public TaskItem Task { get; }
    public TaskCreatedEvent(TaskItem task) => Task = task;
}