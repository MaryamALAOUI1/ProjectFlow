- ProjectFlow

ProjectFlow is a realistic full-stack application built with .NET 9 and Angular.
The backend follows Clean Architecture, implementing CQRS and Domain-Driven Design principles.
This project fulfills the requirements of a technical assessment and demonstrates the ability to design, build, test, and containerize a modern web application.

- Architecture Overview

The backend strictly follows Clean Architecture to ensure separation of concerns, maintainability, and testability:

* Domain: Core business entities (Project, Task), enums (TaskStatus), and domain logic. This layer has no external dependencies.

* Application: Application logic. Implements the CQRS pattern with Commands, Queries, and Handlers (via MediatR). Business rule validation is handled with FluentValidation.

* Infrastructure: External concerns such as data persistence using Entity Framework Core and SQL Server.

* API: ASP.NET Core controllers for handling HTTP requests and responses. Documented with Swagger/OpenAPI.

* The frontend is a single-page Angular application located in the /frontend directory. It communicates with the backend via a RESTful API.

- Tech Stack

* Backend: .NET 9, ASP.NET Core, C#

* Architecture: Clean Architecture, CQRS (MediatR), Domain-Driven Design

* Database: SQL Server

* ORM: Entity Framework Core

* Validation: FluentValidation

* Testing (Backend): xUnit

* Frontend: Angular, TypeScript

* UI Components: PrimeNG

* Containerization: Docker & Docker Compose

# How to Run
- Prerequisites

Docker Desktop (must be running)

.NET 9 SDK

Node.js (LTS) & Angular CLI

1. Run the Backend (Database + API)

The backend is containerized with Docker Compose. From the project root:

docker-compose up --build


This will:

Build the Docker image for the .NET API

Start the SQL Server container

Start the API container once the database is ready

Database: localhost:1433

API: http://localhost:8080

Swagger UI: http://localhost:8080/swagger

If running for the first time, apply migrations:

dotnet ef database update --project ProjectFlow.Infrastructure --startup-project ProjectFlow.Api

2. Run the Frontend

In a new terminal, go to the frontend directory:

cd frontend
npm install
ng serve


Frontend will be available at http://localhost:4200.

Running Tests
Backend Unit Tests

From the project root:

dotnet test


This runs all xUnit tests in the ProjectFlow.Application.UnitTests project.