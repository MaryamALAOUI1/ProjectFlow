using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectFlow.Application.Projects;
using ProjectFlow.Domain;
using ProjectFlow.Domain.Events;
using ProjectFlow.Infrastructure;
using Xunit;
using FluentAssertions;

namespace ProjectFlow.Application.UnitTests;

public class CreateProjectCommandHandlerTests
{
    private readonly ProjectDbContext _context;
    private readonly Mock<IValidator<CreateProjectCommand>> _mockValidator;
    private readonly Mock<IPublisher> _mockPublisher;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _mockPublisher = new Mock<IPublisher>();
        var options = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ProjectDbContext(options, _mockPublisher.Object);
        _mockValidator = new Mock<IValidator<CreateProjectCommand>>();
        _handler = new CreateProjectCommandHandler(_context, _mockValidator.Object);
    }

    [Fact]
    public async Task Handle_GivenValidCommand_ShouldAddProjectToDatabaseAndReturnIt()
    {
        var command = new CreateProjectCommand { Name = "New awesome project" };
        var validationResult = new FluentValidation.Results.ValidationResult();

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("New awesome project");

        var projectInDb = await _context.Projects.FindAsync(result.Id);
        projectInDb.Should().NotBeNull();
        projectInDb?.Name.Should().Be("New awesome project");
    }

    [Fact]
    public async Task Handle_GivenValidCommand_ShouldAddDomainEventToEntity()
    {
        var command = new CreateProjectCommand { Name = "New Event Project" };
        var validationResult = new FluentValidation.Results.ValidationResult();
        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _handler.Handle(command, CancellationToken.None);

        
        result.DomainEvents.Should().HaveCount(1);
        result.DomainEvents.First().Should().BeOfType<ProjectCreatedEvent>();
    }

    [Fact]
    public async Task Handle_GivenInvalidCommand_ShouldThrowValidationException()
    {
        var command = new CreateProjectCommand { Name = "" };
        var validationFailure = new FluentValidation.Results.ValidationFailure("Name", "Name cannot be empty");
        var validationResult = new FluentValidation.Results.ValidationResult(new[] { validationFailure });

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}