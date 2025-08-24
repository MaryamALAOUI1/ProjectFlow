using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Domain;
using ProjectFlow.Domain.Events;
using ProjectFlow.Infrastructure;

namespace ProjectFlow.Application.Projects;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly ProjectDbContext _context;
    private readonly IValidator<UpdateProjectCommand> _validator;

    public UpdateProjectCommandHandler(ProjectDbContext context, IValidator<UpdateProjectCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(request, token);

        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, token);

        if (project == null)
        {
            return;
        }

        project.UpdateName(request.Name);

        project.AddDomainEvent(new ProjectUpdatedEvent(project));

        await _context.SaveChangesAsync(token);
    }
}