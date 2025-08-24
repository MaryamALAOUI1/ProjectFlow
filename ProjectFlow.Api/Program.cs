using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Api.Middleware;
using ProjectFlow.Application.Projects;
using ProjectFlow.Application.Tasks;
using ProjectFlow.Infrastructure;
using System.Reflection; 
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(CreateProjectCommand).Assembly);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddScoped<IValidator<CreateProjectCommand>, CreateProjectCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectCommand>, UpdateProjectCommandValidator>();
builder.Services.AddScoped<IValidator<CreateTaskCommand>, CreateTaskCommandValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();