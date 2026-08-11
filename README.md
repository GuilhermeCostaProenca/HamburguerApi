# Hamburguer API

A clean, well-structured REST API for a burger-ordering domain — built to show solid backend fundamentals in ASP.NET Core.

## Highlights

- **RESTful controllers** for the full domain: burgers, ingredients, extras (drinks, sides, desserts), customers and orders
- **Entity Framework Core** for data access
- **FluentValidation** for request validation
- **Swagger / OpenAPI** documentation out of the box
- **DTOs** separating the API contract from the data model
- Enums serialized as strings, CORS enabled, clean `Program.cs` bootstrap

## Stack

C# · .NET (ASP.NET Core) · Entity Framework Core · FluentValidation · Swagger

## Getting started

```bash
git clone https://github.com/GuilhermeCostaProenca/hamburguer-api.git
cd hamburguer-api
dotnet restore
dotnet run
```

The Swagger UI is available at the app root once it's running.
