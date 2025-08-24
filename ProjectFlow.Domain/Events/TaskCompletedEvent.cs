using ProjectFlow.Domain.Common;

namespace ProjectFlow.Domain.Events;

public class TaskCompletedEvent : BaseEvent
{
    public TaskCompletedEvent(TaskItem taskItem)
    {
        TaskItem = taskItem;
    }

    public TaskItem TaskItem { get; }
}