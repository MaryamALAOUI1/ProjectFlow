using MediatR;
using ProjectFlow.Domain;
namespace ProjectFlow.Application.Projects;
public class GetAllProjectsQuery : IRequest<List<Project>> { }