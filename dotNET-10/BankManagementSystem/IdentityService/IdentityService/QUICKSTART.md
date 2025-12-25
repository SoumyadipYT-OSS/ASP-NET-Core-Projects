# IdentityService.API - Quick Start Guide

## Overview

The IdentityService.API is a complete, production-ready identity and access management microservice built with .NET 10. It provides user authentication, authorization, role management, and comprehensive security features.

## Database Setup (LocalDB)

### 1. Verify LocalDB Connection String

Open `appsettings.json` and verify the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=IdentityServiceDb;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### 2. Apply Migrations (Already Done)

The database has already been created via Entity Framework migrations. The schema includes:

- **AspNetUsers** - User accounts with extended properties
- **AspNetRoles** - Role definitions (Admin, Manager, User)
- **AspNetUserRoles** - User-Role mappings
- **RefreshTokens** - JWT refresh token storage
- **DomainEventLogs** - Event sourcing for auditing

### 3. Verify Database

To check if the database was created:

```bash
# Open SQL Server Object Explorer in Visual Studio
# Connect to: (localdb)\mssqllocaldb
# Database name: IdentityServiceDb
```

## Running the Application

### 1. Run from Visual Studio

```bash
# In Visual Studio, press F5 or Ctrl+F5
# Application will start at: https://localhost:7000
```

### 2. Run from Command Line

```bash
cd IdentityService
dotnet run
```

### 3. Access the API

- **Base URL**: `https://localhost:7000`
- **OpenAPI/Swagger**: `https://localhost:7000/openapi/v1.json`
- **Health Check**: `https://localhost:7000/health`

## Default Admin Account

**Email**: `admin@identityservice.local`
**Password**: `Admin@123456`

?? **Change this password immediately!**

## API Usage Examples

### 1. Register a New User

```bash
curl -X POST https://localhost:7000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePass@123",
    "firstName": "John",
    "lastName": "Doe",
    "dateOfBirth": "1995-06-15"
  }'
```

### 2. Login

```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePass@123"
  }'
```

Response:
```json
{
  "accessToken": "eyJhbGc...",
  "refreshToken": "base64-encoded-token",
  "expiresAt": "2024-01-15T10:30:00Z",
  "success": true,
  "message": "Login successful"
}
```

### 3. Get User Profile (Requires Authentication)

```bash
curl -X GET https://localhost:7000/api/users/user-id \
  -H "Authorization: Bearer eyJhbGc..."
```

### 4. Refresh Token

```bash
curl -X POST https://localhost:7000/api/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "accessToken": "eyJhbGc...",
    "refreshToken": "base64-encoded-token"
  }'
```

### 5. Assign Role (Admin Only)

```bash
curl -X POST https://localhost:7000/api/users/user-id/roles \
  -H "Authorization: Bearer admin-token" \
  -H "Content-Type: application/json" \
  -d '{
    "roleName": "Manager"
  }'
```

### 6. Lock User (Admin Only)

```bash
curl -X POST https://localhost:7000/api/users/user-id/lock \
  -H "Authorization: Bearer admin-token" \
  -H "Content-Type: application/json" \
  -d '{
    "reason": "Suspicious activity detected"
  }'
```

## Configuration

### JWT Settings

Update `appsettings.json` with secure values:

```json
"JwtSettings": {
  "SecretKey": "use-a-secure-key-minimum-32-characters-long",
  "Issuer": "IdentityService",
  "Audience": "IdentityServiceClients",
  "ExpiryMinutes": 15,
  "RefreshTokenExpiryDays": 7
}
```

### Database Configuration

Update connection string for your environment:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=IdentityServiceDb;User Id=sa;Password=your-password;"
}
```

## Password Policy

Users must follow this password policy:

- ? Minimum 8 characters
- ? At least one uppercase letter (A-Z)
- ? At least one lowercase letter (a-z)
- ? At least one digit (0-9)
- ? At least one special character (!@#$%^&*)

Example valid passwords:
- `SecurePass@123`
- `MyP@ssw0rd!`
- `Test#2024Pass`

## Available Roles

1. **Admin** - Full system access, can manage users and roles
2. **Manager** - Supervisory access to users and reports
3. **User** - Standard user role (default for new registrations)

## Logging

Structured logs are written to:

- **Console**: Real-time output (development only)
- **Files**: `logs/identityservice-YYYY-MM-DD.txt`

Logs include:
- Correlation IDs for request tracing
- Machine name and thread ID
- Structured data for easy parsing

## Security Features

- ?? **JWT Tokens** - 15-minute access tokens
- ?? **Refresh Tokens** - 7-day refresh tokens for extended sessions
- ?? **Password Hashing** - bcrypt via ASP.NET Core Identity
- ?? **Account Lockout** - Auto-lock after 5 failed login attempts
- ?? **Role-Based Access** - Fine-grained authorization
- ?? **Audit Logging** - Domain event tracking

## Troubleshooting

### Build Errors

```bash
# Clear build cache
dotnet clean
dotnet build
```

### Database Connection Issues

```bash
# Check LocalDB instances
sqllocaldb info

# Start LocalDB
sqllocaldb start mssqllocaldb

# Recreate migrations
dotnet ef migrations remove
dotnet ef migrations add Initial
dotnet ef database update
```

### Port Already in Use

If port 7000 is in use, change it in `launchSettings.json`:

```json
{
  "profiles": {
    "IdentityService": {
      "applicationUrl": "https://localhost:8443;http://localhost:8000"
    }
  }
}
```

## Testing

### Run Unit Tests

```bash
dotnet test --filter "Category=Unit"
```

### Run Integration Tests

```bash
dotnet test --filter "Category=Integration"
```

### Run All Tests

```bash
dotnet test
```

## Development Workflow

1. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature
   ```

2. **Make changes** following the clean architecture structure

3. **Run tests locally**
   ```bash
   dotnet test
   ```

4. **Commit with descriptive messages**
   ```bash
   git commit -m "feat: add two-factor authentication"
   ```

5. **Push and create PR**
   ```bash
   git push origin feature/your-feature
   ```

## Deployment

### Docker Deployment

```bash
# Build image
docker build -t identityservice:latest .

# Run container
docker run -p 8000:80 \
  -e ConnectionStrings__DefaultConnection="connection-string" \
  -e JwtSettings__SecretKey="your-secret-key" \
  identityservice:latest
```

### Azure Deployment

1. Update `appsettings.Production.json` with Azure SQL credentials
2. Configure Key Vault for secrets
3. Deploy via Azure DevOps or GitHub Actions

## Performance Tips

- ? Enable connection pooling (default)
- ? Use pagination for large datasets
- ? Cache role lookups in-memory if needed
- ? Enable HTTPS compression
- ? Monitor database query performance

## Common Tasks

### Reset Admin Password

```bash
# Connect to the database and run:
UPDATE AspNetUsers 
SET PasswordHash = NULL 
WHERE Email = 'admin@identityservice.local'

# Then use password reset feature in API
```

### Export User Data

```bash
# Use API to query users:
GET /api/users?pageNumber=1&pageSize=100
```

### Generate New JWT Secret

```powershell
# Generate secure key
[Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
```

## Next Steps

1. ? Review the README.md for comprehensive documentation
2. ? Customize the JWT settings for your environment
3. ? Implement custom validators as needed
4. ? Add two-factor authentication (future enhancement)
5. ? Setup CI/CD pipeline for automated deployments

## Support

- ?? Documentation: See `README.md`
- ?? Report Issues: Create GitHub issue
- ?? Discussions: GitHub Discussions or email

## License

MIT License - See LICENSE file
