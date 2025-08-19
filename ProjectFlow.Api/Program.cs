using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Api.Middleware; 
using ProjectFlow.Application.Projects;
using ProjectFlow.Application.Tasks;
using ProjectFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectCommandValidator>();
builder.Services.AddScoped<IValidator<CreateTaskCommand>, CreateTaskCommandValidator>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();