using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Infrastructure;
using FluentValidation;
using ProjectFlow.Application.Projects;
using ProjectFlow.Api.Middleware; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly));

builder.Services.AddControllers();

builder.Services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



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