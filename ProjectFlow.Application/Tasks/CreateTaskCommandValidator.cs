using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Tasks;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    private readonly ProjectDbContext _context;

    public CreateTaskCommandValidator(ProjectDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title cannot be empty.")
            .MaximumLength(200).WithMessage("Task title must not exceed 200 characters.");

        RuleFor(x => x.ProjectId)
            .GreaterThan(0) 
            .MustAsync(ProjectExists).WithMessage("The specified project does not exist.");
    }

    private async Task<bool> ProjectExists(int projectId, CancellationToken cancellationToken)
    {
        return await _context.Projects.AnyAsync(p => p.Id == projectId, cancellationToken);
    }
}