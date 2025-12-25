# IdentityService.API - File Structure

## Complete Directory Layout

```
IdentityService/
?
??? Domain/                              # Domain Layer (Business Logic)
?   ??? Entities/
?   ?   ??? AppUser.cs                  # User aggregate root with domain logic
?   ?   ??? AppRole.cs                  # Role entity (extends IdentityRole<string>)
?   ?   ??? AppUserRole.cs              # Junction entity for User-Role relationship
?   ?   ??? RefreshToken.cs             # Refresh token storage with validation
?   ?
?   ??? Events/
?       ??? DomainEvent.cs              # Abstract base class for domain events
?       ??? UserRegisteredEvent.cs      # Event published on user registration
?       ??? RoleAssignedEvent.cs        # Event published on role assignment
?       ??? UserLockedEvent.cs          # Event published on account lock
?
??? Application/                         # Application Layer (CQRS & Use Cases)
?   ??? Commands/
?   ?   ??? RegisterUserCommand.cs      # Command + Response record
?   ?   ??? RegisterUserCommandValidator.cs
?   ?   ??? LoginCommand.cs
?   ?   ??? LoginCommandValidator.cs
?   ?   ??? RefreshTokenCommand.cs
?   ?   ??? AssignRoleCommand.cs
?   ?   ??? AssignRoleCommandValidator.cs
?   ?   ??? LockUserCommand.cs
?   ?   ??? UnlockUserCommand.cs
?   ?
?   ??? Queries/
?   ?   ??? GetUserByIdQuery.cs         # Query + Response record
?   ?   ??? GetAllUsersQuery.cs         # Paginated query
?   ?
?   ??? Handlers/
?   ?   ??? RegisterUserCommandHandler.cs
?   ?   ??? LoginCommandHandler.cs
?   ?   ??? RefreshTokenCommandHandler.cs
?   ?   ??? AssignRoleCommandHandler.cs
?   ?   ??? LockUserCommandHandler.cs
?   ?   ??? UnlockUserCommandHandler.cs
?   ?   ??? GetUserByIdQueryHandler.cs
?   ?   ??? GetAllUsersQueryHandler.cs
?   ?
?   ??? Behaviors/
?       ??? ValidationBehavior.cs       # MediatR pipeline for FluentValidation
?       ??? LoggingBehavior.cs          # MediatR pipeline for structured logging
?
??? Infrastructure/                      # Infrastructure Layer (Data Access & External Services)
?   ??? Persistence/
?   ?   ??? IdentityDbContext.cs        # EF Core DbContext with Identity support
?   ?   ??? DomainEventLog.cs           # Event sourcing entity
?   ?   ??? Migrations/
?   ?       ??? 20251225092333_IdentityServiceCreate.cs
?   ?       ??? IdentityDbContextModelSnapshot.cs
?   ?
?   ??? Repositories/
?       ??? IUserRepository.cs          # User data access interface
?       ??? UserRepository.cs           # User repository implementation
?       ??? IRefreshTokenRepository.cs  # Refresh token interface
?       ??? RefreshTokenRepository.cs   # Refresh token repository
?
??? API/                                 # API Layer (Controllers & Middleware)
?   ??? Controllers/
?   ?   ??? AuthController.cs           # POST /api/auth/register
?   ?   ?                               # POST /api/auth/login
?   ?   ?                               # POST /api/auth/refresh-token
?   ?   ?
?   ?   ??? UsersController.cs          # GET /api/users/{id}
?   ?                                   # GET /api/users?page=1&size=10
?   ?                                   # POST /api/users/{id}/roles (Admin)
?   ?                                   # POST /api/users/{id}/lock (Admin)
?   ?                                   # POST /api/users/{id}/unlock (Admin)
?   ?
?   ??? Middleware/
?       ??? GlobalExceptionHandlerMiddleware.cs  # Exception handling with ProblemDetails
?
??? Tests/                               # Test Layer
?   ??? Unit/
?   ?   ??? ValidatorsTests.cs          # 30+ unit tests for validators
?   ?
?   ??? Integration/
?   ?   ??? AuthenticationIntegrationTests.cs  # 7+ integration tests
?   ?
?   ??? IdentityService.Tests.csproj
?
??? Migrations/                          # Database migrations (Alternative location)
?   ??? [Generated migration files]
?
??? logs/                                # Log files (created at runtime)
?   ??? identityservice-2024-12-25.txt   # Daily rolling logs
?
??? obj/                                 # Build artifacts (generated)
??? bin/                                 # Compiled binaries (generated)
?
??? Properties/
?   ??? launchSettings.json              # Project launch configuration
?
??? Program.cs                           # Application entry point & DI setup
??? appsettings.json                     # Base configuration (shared)
??? appsettings.Development.json         # Development environment config
??? appsettings.Production.json          # Production environment template
?
??? IdentityService.csproj               # Project file with NuGet references
?
??? README.md                            # Comprehensive API documentation
??? QUICKSTART.md                        # Getting started guide
??? IMPLEMENTATION_SUMMARY.md            # Complete implementation overview
?
??? .gitignore                           # Git ignore file (if present)

```

## File Statistics

### Code Files: ~40 files
- Controllers: 2 files
- CQRS Handlers: 8 files
- Commands/Queries: 15 files
- Validators: 5 files
- Domain Entities: 5 files
- Domain Events: 4 files
- Repositories: 4 files
- Middleware: 1 file
- DbContext: 1 file
- Tests: 2 files
- Configuration: 3 files

### Lines of Code
- Domain Layer: ~400 LOC
- Application Layer: ~1,200 LOC
- Infrastructure Layer: ~500 LOC
- API Layer: ~300 LOC
- Tests: ~600 LOC
- **Total: ~3,000 LOC**

### Database Schema
- **8 Tables** (including Identity tables)
- **12 Indexes** (optimized for queries)
- **Foreign Key Relationships** (referential integrity)
- **Default Values** (SQL-level defaults)

## NuGet Packages: 13 Total

```
Microsoft.AspNetCore.Identity.EntityFrameworkCore     (10.0.1)
Microsoft.AspNetCore.Authentication.JwtBearer         (10.0.1)
Microsoft.AspNetCore.OpenApi                          (10.0.1)
Microsoft.EntityFrameworkCore.SqlServer               (10.0.1)
Microsoft.EntityFrameworkCore.Tools                   (10.0.1)
Microsoft.IdentityModel.Tokens                        (8.15.0)
Microsoft.Identity.Web                                (4.2.0)
MediatR                                               (14.0.0)
FluentValidation                                      (12.1.1)
Serilog.AspNetCore                                    (10.0.0)
Serilog.Sinks.File                                    (7.0.0)
AspNetCore.HealthChecks.SqlServer                     (9.0.0)
```

## Key Files Description

### Core Application Files

| File | Purpose | Lines |
|------|---------|-------|
| Program.cs | Service registration, middleware setup, DB seeding | ~170 |
| IdentityDbContext.cs | EF Core context, entity configs, seed data | ~130 |
| AppUser.cs | User aggregate with domain logic | ~70 |
| AuthController.cs | Register, Login, RefreshToken endpoints | ~80 |
| UsersController.cs | User management, roles, lock/unlock | ~120 |

### CQRS Handlers

| Handler | Purpose | Lines |
|---------|---------|-------|
| RegisterUserCommandHandler.cs | User registration with domain events | ~70 |
| LoginCommandHandler.cs | Authentication with JWT token generation | ~95 |
| RefreshTokenCommandHandler.cs | Token refresh with old token revocation | ~85 |
| AssignRoleCommandHandler.cs | Role assignment with domain events | ~60 |

### Validators

| Validator | Purpose | Rules |
|-----------|---------|-------|
| RegisterUserCommandValidator | Email, password, names, age | 10+ rules |
| LoginCommandValidator | Email, password | 2 rules |
| AssignRoleCommandValidator | User ID, role name | 2 rules |

## Configuration Files

### appsettings.json (Base)
```json
{
  "ConnectionStrings": { "DefaultConnection": "..." },
  "JwtSettings": { ... },
  "Serilog": { ... },
  "Logging": { ... },
  "AllowedHosts": "*"
}
```

### appsettings.Development.json
- Debug-level logging
- Verbose database queries
- Console output enabled

### appsettings.Production.json
- Information-level logging
- File output only
- Secure connection string template

## Database Entities

### Identity Tables (7 tables)
- AspNetUsers - Users with email, roles
- AspNetRoles - Role definitions
- AspNetUserRoles - User-role mappings
- AspNetUserClaims - User claims
- AspNetUserLogins - External logins
- AspNetRoleClaims - Role claims
- AspNetUserTokens - User tokens

### Custom Tables (2 tables)
- RefreshTokens - Refresh token storage with expiry
- DomainEventLogs - Event sourcing for auditing

## Endpoint Summary

### Public Endpoints
```
POST   /api/auth/register           - User registration
POST   /api/auth/login              - User authentication
POST   /api/auth/refresh-token      - Token refresh
GET    /health                      - Health check
GET    /openapi/v1.json            - OpenAPI schema
```

### Authenticated Endpoints
```
GET    /api/users/{id}              - Get user profile
GET    /api/users                   - List users (paginated)
```

### Admin-Only Endpoints
```
POST   /api/users/{id}/roles        - Assign role to user
POST   /api/users/{id}/lock         - Lock user account
POST   /api/users/{id}/unlock       - Unlock user account
```

## Build Configuration

### Target Framework
- .NET 10 (net10.0)
- C# 14.0 language features

### Dependencies
- ASP.NET Core 10.0
- Entity Framework Core 10.0
- ASP.NET Core Identity 10.0

### Compilation
- Full nullable reference types
- Implicit using statements
- Web SDK enabled

---

## How to Navigate the Codebase

1. **Start with**: `Program.cs` - See the complete DI setup
2. **Then review**: `AuthController.cs` - Understand API endpoints
3. **Explore**: Domain entities (`AppUser.cs`) - See business logic
4. **Study**: Command handlers (`RegisterUserCommandHandler.cs`) - See CQRS flow
5. **Check**: `IdentityDbContext.cs` - Understand data mapping
6. **Test**: Run `dotnet test` to see validation and integration tests

---

## Development Tips

### Adding a New Feature
1. Create domain entity if needed (Domain/)
2. Create command/query (Application/Commands or Queries/)
3. Create validator (Application/Commands/)
4. Create handler (Application/Handlers/)
5. Add controller endpoint (API/Controllers/)
6. Create/update migration: `dotnet ef migrations add FeatureName`
7. Update database: `dotnet ef database update`
8. Add tests (Tests/)

### Running the Application
```bash
dotnet run
# API: https://localhost:7000
# OpenAPI: https://localhost:7000/openapi/v1.json
# Health: https://localhost:7000/health
```

### Running Tests
```bash
dotnet test                                    # All tests
dotnet test --filter "Category=Unit"           # Unit tests only
dotnet test --filter "Category=Integration"    # Integration tests only
```

### Checking Database
```bash
# Via SQL Server Object Explorer:
# (localdb)\mssqllocaldb -> IdentityServiceDb

# Via dotnet:
dotnet ef dbcontext info
```

---

*This file structure represents a production-ready microservice following clean architecture and CQRS patterns.*
