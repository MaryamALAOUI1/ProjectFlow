using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Infrastructure;
namespace ProjectFlow.Application.Projects;
public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    private readonly ProjectDbContext _context;
    public UpdateProjectCommandValidator(ProjectDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name)
            .NotEmpty().MaximumLength(100);
        RuleFor(x => x).MustAsync(BeUniqueName).WithMessage("A project with this name already exists.");
    }
    private async Task<bool> BeUniqueName(UpdateProjectCommand command, CancellationToken token)
    {
        return !await _context.Projects.AnyAsync(p => p.Id != command.Id && p.Name == command.Name, token);
    }
}