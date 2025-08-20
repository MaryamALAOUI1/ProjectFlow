using MediatR;
using ProjectFlow.Domain;

namespace ProjectFlow.Application.Tasks;

public class UpdateTaskDetailsCommand : IRequest
{
    public int TaskId { get; set; }
    public Domain.TaskStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
}