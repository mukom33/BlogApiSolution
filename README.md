# BlogApiSolution

A backend-focused ASP.NET Core Web API project built with a layered architecture. This project is designed to manage a blog system with features such as posts, comments, tags, likes, user authentication, and authorization.

## Features

- RESTful API architecture
- ASP.NET Core Web API
- Layered architecture
- Entity Framework Core
- PostgreSQL integration
- ASP.NET Identity
- JWT Authentication
- Role-based Authorization
- CRUD operations
- DTO usage
- Repository pattern
- AutoMapper integration
- Pagination support
- Swagger / OpenAPI documentation

## Project Structure

The solution consists of the following layers:

- **BlogApi.API**  
  Handles controllers, authentication setup, program configuration, and API endpoints.

- **BlogApi.Business**  
  Contains business logic, services, DTOs, and AutoMapper profiles.

- **BlogApi.DataAccess**  
  Includes DbContext, migrations, and repository implementations.

- **BlogApi.Domain**  
  Contains core entities and shared models.

## Technologies Used

- C#
- .NET / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- ASP.NET Identity
- JWT
- AutoMapper
- Swagger

## Main Entities

- AppUser
- AppRole
- Post
- Comment
- Tag
- PostLike

## Authentication and Authorization

This project includes:

- User registration and login
- JWT token generation
- ASP.NET Identity integration
- Protected endpoints with `[Authorize]`
- Role-based access control

## API Capabilities

Implemented API capabilities include:

- Creating, updating, deleting, and listing posts
- Adding and managing comments
- Managing tags
- Managing post likes
- User-related operations
- Pagination support for selected endpoints

## Getting Started

### Prerequisites

Make sure you have installed:

- .NET SDK
- PostgreSQL
- Visual Studio / Visual Studio Code

### Clone the repository

```bash
git clone https://github.com/mukom33/BlogApiSolution.git
cd BlogApiSolution
