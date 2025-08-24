using Xunit;
using ProjectFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Domain;
using ProjectFlow.Application.Projects;
using FluentAssertions;
using Moq;
using MediatR;

namespace ProjectFlow.Application.UnitTests;

public class DeleteProjectCommandHandlerTests
{
    private readonly ProjectDbContext _context;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var publisher = new Mock<IPublisher>().Object;

        _context = new ProjectDbContext(options, publisher);

        _handler = new DeleteProjectCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_GivenExistingProjectId_ShouldRemoveProjectFromDatabase()
    {
        var projectToDelete = Project.Create("Project to Delete");  
        _context.Projects.Add(projectToDelete);
        await _context.SaveChangesAsync();

        var command = new DeleteProjectCommand { Id = projectToDelete.Id };

        await _handler.Handle(command, CancellationToken.None);

        var projectInDb = await _context.Projects.FindAsync(projectToDelete.Id);
        projectInDb.Should().BeNull();
    }


    [Fact]
    public async Task Handle_GivenNonExistentProjectId_ShouldDoNothing()
    {
       
        var command = new DeleteProjectCommand { Id = 999 };

        
        await _handler.Handle(command, CancellationToken.None);

   
        var count = await _context.Projects.CountAsync();
        count.Should().Be(0);
    }
}