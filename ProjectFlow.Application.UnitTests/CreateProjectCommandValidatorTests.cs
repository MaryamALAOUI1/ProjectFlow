using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Application.Projects;
using ProjectFlow.Domain;
using ProjectFlow.Infrastructure;
using Xunit;

namespace ProjectFlow.Application.UnitTests;

public class CreateProjectCommandValidatorTests
{
    private readonly ProjectDbContext _context;
    private readonly CreateProjectCommandValidator _validator;

    public CreateProjectCommandValidatorTests()
    {
       
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ProjectDbContext(options);
        _validator = new CreateProjectCommandValidator(_context);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new CreateProjectCommand { Name = "" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_Not_Have_Error_When_Name_Is_Valid_And_Unique()
    {
        var command = new CreateProjectCommand { Name = "Unique Project" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task Should_Have_Error_When_Name_Already_Exists()
    {
        
        var existingProject = new Project { Name = "Existing Project" };
        _context.Projects.Add(existingProject);
        await _context.SaveChangesAsync();

        var command = new CreateProjectCommand { Name = "Existing Project" };
        var result = await _validator.TestValidateAsync(command);

       
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage("A project with this name already exists.");
    }
}