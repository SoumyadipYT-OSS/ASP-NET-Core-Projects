# IdentityService.API - .NET 10 Identity Management Microservice

A comprehensive identity and access management service built with .NET 10, following clean architecture principles, CQRS pattern with MediatR, and Entity Framework Core.

## Features

- ? User Registration with strong password validation
- ? User Login with JWT token generation
- ? Refresh Token mechanism for session management
- ? Role-based Access Control (RBAC)
- ? User Lock/Unlock functionality
- ? Failed login attempt tracking
- ? Domain-driven design with domain events
- ? Structured logging with Serilog
- ? Health checks for SQL Server
- ? Exception handling with ProblemDetails
- ? Comprehensive unit and integration tests

## Architecture

### Project Structure

```
IdentityService/
??? Domain/                          # Domain layer (entities, events, aggregates)
?   ??? Entities/
?   ?   ??? AppUser.cs             # User aggregate
?   ?   ??? AppRole.cs             # Role entity
?   ?   ??? AppUserRole.cs         # User-Role mapping
?   ?   ??? RefreshToken.cs        # Refresh token entity
?   ??? Events/
?       ??? DomainEvent.cs         # Base domain event
?       ??? UserRegisteredEvent.cs
?       ??? RoleAssignedEvent.cs
?       ??? UserLockedEvent.cs
??? Application/                    # Application layer (CQRS, validators)
?   ??? Commands/
?   ?   ??? RegisterUserCommand.cs
?   ?   ??? LoginCommand.cs
?   ?   ??? RefreshTokenCommand.cs
?   ?   ??? AssignRoleCommand.cs
?   ?   ??? LockUserCommand.cs
?   ?   ??? UnlockUserCommand.cs
?   ??? Queries/
?   ?   ??? GetUserByIdQuery.cs
?   ?   ??? GetAllUsersQuery.cs
?   ??? Handlers/                  # CQRS command/query handlers
?   ??? Validators/                # FluentValidation validators
?   ??? Behaviors/                 # MediatR pipeline behaviors
??? Infrastructure/                # Infrastructure layer (EF Core, repositories)
?   ??? Persistence/
?   ?   ??? IdentityDbContext.cs
?   ?   ??? DomainEventLog.cs
?   ??? Repositories/
?       ??? UserRepository.cs
?       ??? RefreshTokenRepository.cs
??? API/                          # API layer (controllers, middleware)
?   ??? Controllers/
?   ?   ??? AuthController.cs     # Authentication endpoints
?   ?   ??? UsersController.cs    # User management endpoints
?   ??? Middleware/
?       ??? GlobalExceptionHandlerMiddleware.cs
??? Tests/
?   ??? Integration/              # Integration tests
?   ??? Unit/                     # Unit tests
??? Program.cs                    # Service configuration
??? appsettings.json             # Configuration

```

### Architecture Principles

- **Clean Architecture**: Separation of concerns across layers
- **CQRS**: Command Query Responsibility Segregation pattern
- **MediatR**: Mediator pattern for loose coupling
- **Domain-Driven Design**: Rich domain models with aggregates and domain events
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: IoC container for service management
- **Validation Pipeline**: FluentValidation integrated via MediatR behavior

## Technology Stack

- **.NET 10**: Latest .NET runtime
- **ASP.NET Core**: Web framework
- **Entity Framework Core 10**: ORM with SQL Server provider
- **ASP.NET Core Identity**: User and role management
- **JWT Bearer**: Token-based authentication
- **MediatR 14**: CQRS and mediator pattern
- **FluentValidation 12**: Input validation
- **Serilog 10**: Structured logging
- **xUnit**: Unit testing framework
- **SQL Server**: Database provider

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd IdentityService
   ```

2. **Configure Connection String**
   
   Update `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=IdentityServiceDb;Trusted_Connection=true;"
   }
   ```

3. **Configure JWT Settings**
   
   Update `appsettings.json` with a secure secret key (min 32 characters):
   ```json
   "JwtSettings": {
     "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long!!!",
     "Issuer": "IdentityService",
     "Audience": "IdentityServiceClients"
   }
   ```

4. **Apply Database Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7000`

## API Endpoints

### Authentication

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass@123",
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "2000-01-15"
}

Response: 201 Created
{
  "userId": "uuid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "success": true,
  "message": "User registered successfully"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass@123"
}

Response: 200 OK
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "base64-encoded-token",
  "expiresAt": "2024-01-15T10:30:00Z",
  "success": true,
  "message": "Login successful"
}
```

#### Refresh Token
```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "accessToken": "eyJhbGc...",
  "refreshToken": "base64-encoded-token"
}

Response: 200 OK
{
  "newAccessToken": "eyJhbGc...",
  "newRefreshToken": "base64-encoded-token",
  "expiresAt": "2024-01-15T10:45:00Z",
  "success": true,
  "message": "Token refreshed successfully"
}
```

### User Management (Requires Authentication)

#### Get User by ID
```http
GET /api/users/{userId}
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "userId": "uuid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "createdAt": "2024-01-10T08:00:00Z",
  "roles": ["User"]
}
```

#### Get All Users (Paginated)
```http
GET /api/users?pageNumber=1&pageSize=10
Authorization: Bearer {accessToken}

Response: 200 OK
{
  "users": [
    {
      "userId": "uuid",
      "email": "user@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "createdAt": "2024-01-10T08:00:00Z",
      "roles": ["User"]
    }
  ],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10
}
```

#### Assign Role (Admin Only)
```http
POST /api/users/{userId}/roles
Authorization: Bearer {adminAccessToken}
Content-Type: application/json

{
  "roleName": "Admin"
}

Response: 200 OK
{
  "success": true,
  "message": "Role Admin assigned successfully"
}
```

#### Lock User (Admin Only)
```http
POST /api/users/{userId}/lock
Authorization: Bearer {adminAccessToken}
Content-Type: application/json

{
  "reason": "Suspicious activity detected"
}

Response: 200 OK
{
  "success": true,
  "message": "User locked successfully"
}
```

#### Unlock User (Admin Only)
```http
POST /api/users/{userId}/unlock
Authorization: Bearer {adminAccessToken}

Response: 200 OK
{
  "success": true,
  "message": "User unlocked successfully"
}
```

### Health Check

```http
GET /health

Response: 200 OK
Healthy
```

## Default Admin User

Upon first run, a default admin account is created:

- **Email**: `admin@identityservice.local`
- **Password**: `Admin@123456`

?? **Change this password immediately in production!**

## Password Requirements

- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character

## Role-Based Access Control

### Available Roles

- **Admin**: Full system access, can manage users and roles
- **Manager**: Supervisory access
- **User**: Standard user role

## Error Handling

The API returns standardized error responses using ProblemDetails format:

```json
{
  "status": 400,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "detail": "See the errors property for details.",
  "errors": {
    "email": ["Email must be a valid email address"],
    "password": ["Password must be at least 8 characters"]
  },
  "traceId": "correlation-id"
}
```

## Security Features

- **JWT Tokens**: Secure token-based authentication with 15-minute expiration
- **Refresh Tokens**: 7-day refresh tokens for extended sessions
- **Password Hashing**: ASP.NET Core Identity with bcrypt hashing
- **Failed Login Tracking**: Automatic account lockout after 5 failed attempts
- **Role-Based Authorization**: Fine-grained access control
- **HTTPS**: Required in production
- **CORS**: Configurable cross-origin resource sharing

## Logging

Structured logging is configured using Serilog:

- **Console Output**: Development environment
- **File Output**: `logs/identityservice-YYYY-MM-DD.txt`
- **Enrichment**: Machine name, thread ID, timestamp
- **Correlation IDs**: Trace request through the system

View logs in real-time:
```bash
tail -f logs/identityservice-*.txt
```

## Testing

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Suite
```bash
dotnet test --filter "Category=Integration"
```

### Test Coverage

- **Integration Tests**: Full CQRS workflow testing
- **Unit Tests**: Validator and business logic testing
- **In-Memory Database**: Isolated test environment

## Database Migrations

### Create Migration
```bash
dotnet ef migrations add MigrationName
```

### Apply Migration
```bash
dotnet ef database update
```

### Revert Migration
```bash
dotnet ef database update PreviousMigrationName
```

## Configuration

### appsettings.json Structure

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "JwtSettings": {
    "SecretKey": "...",
    "Issuer": "...",
    "Audience": "...",
    "ExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [...],
    "Enrich": [...]
  }
}
```

## Performance Considerations

- **Connection Pooling**: Enabled by default with EF Core
- **Query Optimization**: Repository pattern for efficient data access
- **Async/Await**: Non-blocking I/O operations
- **Caching**: Refresh tokens cached in database
- **Health Checks**: Database connectivity monitoring

## Deployment

### Docker

Create a `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "IdentityService.dll"]
```

Build and run:
```bash
docker build -t identityservice .
docker run -p 8000:80 identityservice
```

### Azure SQL Database

Update connection string:
```
Server=tcp:servername.database.windows.net,1433;Initial Catalog=IdentityServiceDb;Persist Security Info=False;User ID=sqladmin;Password=yourpassword;Encrypt=True;Connection Timeout=30;
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit changes with clear messages
4. Push to branch
5. Create a Pull Request

## License

MIT License - See LICENSE file for details

## Support

For issues, questions, or suggestions:
- Create an issue on GitHub
- Contact: support@identityservice.local

## Roadmap

- [ ] Azure AD/B2C integration
- [ ] Two-factor authentication (2FA)
- [ ] Social login providers
- [ ] SAML 2.0 support
- [ ] Advanced audit logging
- [ ] Rate limiting
- [ ] API versioning
- [ ] OpenAPI/Swagger documentation
