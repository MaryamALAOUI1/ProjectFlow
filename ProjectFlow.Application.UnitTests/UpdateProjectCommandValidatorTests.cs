using FluentValidation.TestHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectFlow.Application.Projects;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;
using Xunit;

namespace ProjectFlow.Application.UnitTests;

public class UpdateProjectCommandValidatorTests
{
    private readonly ProjectDbContext _context;
    private readonly UpdateProjectCommandValidator _validator;

    public UpdateProjectCommandValidatorTests()
    {
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var publisher = new Mock<IPublisher>().Object;
        _context = new ProjectDbContext(options, publisher);
        _validator = new UpdateProjectCommandValidator(_context);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Duplicate_Of_Another_Project()
    {
        var project1 = Project.Create("Project One");
        var project2 = Project.Create("Project Two");

        _context.Projects.AddRange(project1, project2);
        await _context.SaveChangesAsync();

        var command = new UpdateProjectCommand { Id = project1.Id, Name = "Project Two" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x)
              .WithErrorMessage("A project with this name already exists.");
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Name_Is_Unchanged()
    {
        var project1 = Project.Create("Project One");
        _context.Projects.Add(project1);
        await _context.SaveChangesAsync();

        var command = new UpdateProjectCommand { Id = project1.Id, Name = "Project One" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x);
    }
}