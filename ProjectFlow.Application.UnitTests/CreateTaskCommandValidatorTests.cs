using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Application.Tasks;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;
using Xunit;
using MediatR; 
using Moq;    

namespace ProjectFlow.Application.UnitTests;

public class CreateTaskCommandValidatorTests
{
    private readonly ProjectDbContext _context;
    private readonly CreateTaskCommandValidator _validator;

    public CreateTaskCommandValidatorTests()
    {
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;


        var mockPublisher = new Mock<IPublisher>();
        _context = new ProjectDbContext(options, mockPublisher.Object);

        _validator = new CreateTaskCommandValidator(_context);
    }

    [Fact]
    public async Task Should_Have_Error_When_ProjectId_Does_Not_Exist()
    {
        var command = new CreateTaskCommand { Title = "Test Task", ProjectId = 999 };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(x => x.ProjectId)
              .WithErrorMessage("The specified project does not exist.");
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_ProjectId_Exists()
    {
        var project = new Project { Name = "Existing Project" };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var command = new CreateTaskCommand { Title = "Test Task", ProjectId = project.Id };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(x => x.ProjectId);
    }
}