namespace ProjectFlow.Domain;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "To Do";
    public DateTime? DueDate { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}