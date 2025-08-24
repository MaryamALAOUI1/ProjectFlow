using ProjectFlow.Domain;
using ProjectFlow.Domain.Events;

namespace ProjectFlow.Domain.UnitTests;

public class TaskItemTests
{
    [Fact]
    public void Create_Should_ReturnTaskItem_WhenTitleIsValid()
    {
        
        var validTitle = "Implement the login page";
        var projectId = 1;

        var taskItem = TaskItem.Create(validTitle, projectId);

        Assert.NotNull(taskItem);
        Assert.Equal(validTitle, taskItem.Title);
        Assert.Equal(projectId, taskItem.ProjectId);
        Assert.Equal(TaskStatus.ToDo, taskItem.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_ThrowArgumentException_WhenTitleIsInvalid(string invalidTitle)
    {
        var projectId = 1;

        var exception = Assert.Throws<ArgumentException>(() => TaskItem.Create(invalidTitle, projectId));
        Assert.Equal("Task title cannot be empty. (Parameter 'title')", exception.Message);
    }

    [Fact]
    public void Complete_Should_SetStatusToDone_And_RaiseEvent()
    {
        var taskItem = TaskItem.Create("A task to be completed", 1);

        taskItem.Complete();

        Assert.Equal(TaskStatus.Done, taskItem.Status);
        Assert.Single(taskItem.DomainEvents);
        Assert.IsType<TaskCompletedEvent>(taskItem.DomainEvents.First());
    }

    [Fact]
    public void UpdateDetails_Should_ThrowArgumentException_WhenNewTitleIsInvalid()
    {
        var taskItem = TaskItem.Create("Initial title", 1);
        var invalidNewTitle = "";

        Assert.Throws<ArgumentException>(() => taskItem.UpdateDetails(invalidNewTitle, "some description"));
    }
}