using ProjectFlow.Domain.Common; 

namespace ProjectFlow.Domain;

public class Project : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }

    public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}