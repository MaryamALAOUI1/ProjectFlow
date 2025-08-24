using FluentValidation;
using MediatR;
using ProjectFlow.Domain;
using ProjectFlow.Domain.Events;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Projects;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
{
    private readonly ProjectDbContext _context;
    private readonly IValidator<CreateProjectCommand> _validator;

    public CreateProjectCommandHandler(ProjectDbContext context, IValidator<CreateProjectCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var project = new Project
        {
            Name = request.Name,
            CreationDate = DateTime.UtcNow
        };

        project.AddDomainEvent(new ProjectCreatedEvent(project));
        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return project;
    }
}