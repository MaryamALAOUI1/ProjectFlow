using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Projects;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly ProjectDbContext _context;

    public CreateProjectCommandValidator(ProjectDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name cannot be empty.")
            .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("A project with this name already exists.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Name == name, cancellationToken);

        return !projectExists;
    }
}