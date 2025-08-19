using MediatR;
using ProjectFlow.Domain;

namespace ProjectFlow.Application.Tasks;

public class CreateTaskCommand : IRequest<TaskItem>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ProjectId { get; set; } 
}