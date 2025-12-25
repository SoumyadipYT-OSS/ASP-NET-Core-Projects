# IdentityService.API - Complete API Reference

## Base URL

```
https://localhost:7000  (Development)
https://api.example.com (Production)
```

## Authentication

All endpoints except `/api/auth/register`, `/api/auth/login`, and `/health` require a Bearer token:

```
Authorization: Bearer {accessToken}
```

## Status Codes

- `200 OK` - Successful request
- `201 Created` - Resource created successfully
- `400 Bad Request` - Validation or invalid request
- `401 Unauthorized` - Missing or invalid authentication
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

## Authentication Endpoints

### 1. Register User

**Endpoint**: `POST /api/auth/register`

**Authentication**: Not required

**Request Body**:
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePass@123",
  "firstName": "John",
  "lastName": "Doe",
  "dateOfBirth": "1995-06-15"
}
```

**Password Requirements**:
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character (!@#$%^&*)

**Success Response (201 Created)**:
```json
{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "success": true,
  "message": "User registered successfully"
}
```

**Error Response (400 Bad Request)**:
```json
{
  "status": 400,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "errors": {
    "email": ["Email must be a valid email address"],
    "password": ["Password must contain at least one special character"]
  },
  "traceId": "0HMVV4GKT0QD5:00000001"
}
```

**cURL Example**:
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

---

### 2. Login

**Endpoint**: `POST /api/auth/login`

**Authentication**: Not required

**Request Body**:
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePass@123"
}
```

**Success Response (200 OK)**:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "FPqzb/XrsYSGwfXBFMSSTD4kM0qK8n5wnF...",
  "expiresAt": "2024-12-25T15:15:00Z",
  "success": true,
  "message": "Login successful"
}
```

**Error Response (401 Unauthorized)**:
```json
{
  "status": 401,
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "detail": "Invalid email or password",
  "traceId": "0HMVV4GKT0QD5:00000002"
}
```

**Token Expiration**: 15 minutes for access token, 7 days for refresh token

**cURL Example**:
```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "SecurePass@123"
  }'
```

---

### 3. Refresh Token

**Endpoint**: `POST /api/auth/refresh-token`

**Authentication**: Not required

**Request Body**:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "FPqzb/XrsYSGwfXBFMSSTD4kM0qK8n5wnF..."
}
```

**Success Response (200 OK)**:
```json
{
  "newAccessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "newRefreshToken": "xK9nF5wn4kM0K8STD4MqSSTwFfBXrsY...",
  "expiresAt": "2024-12-25T15:30:00Z",
  "success": true,
  "message": "Token refreshed successfully"
}
```

**Error Response (401 Unauthorized)**:
```json
{
  "status": 401,
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "detail": "Invalid or expired refresh token",
  "traceId": "0HMVV4GKT0QD5:00000003"
}
```

**cURL Example**:
```bash
curl -X POST https://localhost:7000/api/auth/refresh-token \
  -H "Content-Type: application/json" \
  -d '{
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "FPqzb/XrsYSGwfXBFMSSTD4kM0qK8n5wnF..."
  }'
```

---

## User Management Endpoints

### 4. Get User by ID

**Endpoint**: `GET /api/users/{userId}`

**Authentication**: Required

**Path Parameters**:
- `userId` (string, required) - User ID (UUID)

**Success Response (200 OK)**:
```json
{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "createdAt": "2024-12-25T10:00:00Z",
  "roles": ["User"]
}
```

**Error Response (404 Not Found)**:
```json
{
  "status": 404,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "The specified resource was not found.",
  "traceId": "0HMVV4GKT0QD5:00000004"
}
```

**cURL Example**:
```bash
curl -X GET https://localhost:7000/api/users/550e8400-e29b-41d4-a716-446655440000 \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### 5. Get All Users (Paginated)

**Endpoint**: `GET /api/users`

**Authentication**: Required

**Query Parameters**:
- `pageNumber` (integer, optional, default: 1) - Page number (1-based)
- `pageSize` (integer, optional, default: 10) - Results per page (1-100)

**Success Response (200 OK)**:
```json
{
  "users": [
    {
      "userId": "550e8400-e29b-41d4-a716-446655440000",
      "email": "john.doe@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "isActive": true,
      "createdAt": "2024-12-25T10:00:00Z",
      "roles": ["User"]
    },
    {
      "userId": "660e8400-e29b-41d4-a716-446655440001",
      "email": "jane.smith@example.com",
      "firstName": "Jane",
      "lastName": "Smith",
      "isActive": true,
      "createdAt": "2024-12-25T10:05:00Z",
      "roles": ["Manager"]
    }
  ],
  "totalCount": 50,
  "pageNumber": 1,
  "pageSize": 10
}
```

**cURL Example**:
```bash
curl -X GET "https://localhost:7000/api/users?pageNumber=1&pageSize=20" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

---

### 6. Assign Role to User

**Endpoint**: `POST /api/users/{userId}/roles`

**Authentication**: Required (Admin role)

**Path Parameters**:
- `userId` (string, required) - User ID (UUID)

**Request Body**:
```json
{
  "roleName": "Manager"
}
```

**Available Roles**:
- `Admin` - Full system access
- `Manager` - Supervisory access
- `User` - Standard user role

**Success Response (200 OK)**:
```json
{
  "success": true,
  "message": "Role Manager assigned successfully"
}
```

**Error Response (400 Bad Request)**:
```json
{
  "status": 400,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "detail": "Role does not exist",
  "traceId": "0HMVV4GKT0QD5:00000005"
}
```

**Error Response (403 Forbidden)**:
```json
{
  "status": 403,
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
  "title": "Forbidden",
  "detail": "You do not have permission to perform this action",
  "traceId": "0HMVV4GKT0QD5:00000006"
}
```

**cURL Example**:
```bash
curl -X POST https://localhost:7000/api/users/550e8400-e29b-41d4-a716-446655440000/roles \
  -H "Authorization: Bearer admin-token" \
  -H "Content-Type: application/json" \
  -d '{
    "roleName": "Manager"
  }'
```

---

### 7. Lock User Account

**Endpoint**: `POST /api/users/{userId}/lock`

**Authentication**: Required (Admin role)

**Path Parameters**:
- `userId` (string, required) - User ID (UUID)

**Request Body**:
```json
{
  "reason": "Suspicious activity detected"
}
```

**Success Response (200 OK)**:
```json
{
  "success": true,
  "message": "User locked successfully"
}
```

**Effects**:
- User cannot login
- User is marked as locked in the system
- Reason is recorded for audit purposes

**cURL Example**:
```bash
curl -X POST https://localhost:7000/api/users/550e8400-e29b-41d4-a716-446655440000/lock \
  -H "Authorization: Bearer admin-token" \
  -H "Content-Type: application/json" \
  -d '{
    "reason": "Suspicious activity detected"
  }'
```

---

### 8. Unlock User Account

**Endpoint**: `POST /api/users/{userId}/unlock`

**Authentication**: Required (Admin role)

**Path Parameters**:
- `userId` (string, required) - User ID (UUID)

**Success Response (200 OK)**:
```json
{
  "success": true,
  "message": "User unlocked successfully"
}
```

**Effects**:
- User can login again
- Failed login count is reset
- Account is active

**cURL Example**:
```bash
curl -X POST https://localhost:7000/api/users/550e8400-e29b-41d4-a716-446655440000/unlock \
  -H "Authorization: Bearer admin-token"
```

---

## Health Check Endpoint

### 9. Health Check

**Endpoint**: `GET /health`

**Authentication**: Not required

**Success Response (200 OK)**:
```
Healthy
```

**Purpose**: Docker/Kubernetes liveness and readiness probes

**cURL Example**:
```bash
curl -X GET https://localhost:7000/health
```

---

## OpenAPI/Swagger

**Endpoint**: `GET /openapi/v1.json`

**Description**: OpenAPI schema in JSON format, automatically generated from code

**Usage**: Import into tools like Postman, Insomnia, or Swagger UI

---

## Common Error Scenarios

### 1. Missing Authorization Header

**Status**: 401 Unauthorized

```json
{
  "status": 401,
  "title": "Unauthorized",
  "detail": "Authorization header is missing"
}
```

### 2. Expired Token

**Status**: 401 Unauthorized

```json
{
  "status": 401,
  "title": "Unauthorized",
  "detail": "Token has expired. Use refresh-token endpoint."
}
```

### 3. Invalid Token

**Status**: 401 Unauthorized

```json
{
  "status": 401,
  "title": "Unauthorized",
  "detail": "Invalid token signature"
}
```

### 4. User Already Exists

**Status**: 400 Bad Request

```json
{
  "status": 400,
  "title": "Bad Request",
  "detail": "User with this email already exists"
}
```

### 5. User Not Found

**Status**: 404 Not Found

```json
{
  "status": 404,
  "title": "Not Found",
  "detail": "User not found"
}
```

### 6. Insufficient Permissions

**Status**: 403 Forbidden

```json
{
  "status": 403,
  "title": "Forbidden",
  "detail": "You do not have permission to perform this action"
}
```

### 7. Account Locked

**Status**: 401 Unauthorized

```json
{
  "status": 401,
  "title": "Unauthorized",
  "detail": "User account is locked"
}
```

### 8. Account Inactive

**Status**: 401 Unauthorized

```json
{
  "status": 401,
  "title": "Unauthorized",
  "detail": "User account is inactive"
}
```

---

## Rate Limiting

Currently no rate limiting is implemented. Consider adding:

- 5 login attempts per minute per IP
- 10 API calls per second per user
- 100 requests per hour for unauthenticated endpoints

---

## CORS Configuration

Currently, all origins are allowed (`AllowedHosts: "*"`). In production, update:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins",
        policy =>
        {
            policy.WithOrigins("https://example.com")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});
```

---

## Pagination

For list endpoints, pagination uses:

- **pageNumber**: Starting at 1 (default: 1)
- **pageSize**: Records per page (default: 10, max: 100)

**Example**:
```
GET /api/users?pageNumber=2&pageSize=25
```

Returns records 26-50

---

## Timestamps

All timestamps are in UTC (ISO 8601 format):

```
2024-12-25T15:30:00Z
```

---

## Testing Endpoints with Postman

1. **Create Collection** - Name: "IdentityService API"

2. **Set Variables**:
   - `baseUrl`: `https://localhost:7000`
   - `accessToken`: (auto-filled from login)

3. **Create Requests**:
   - Register User (POST /api/auth/register)
   - Login (POST /api/auth/login)
   - Get Profile (GET /api/users/{userId})
   - Assign Role (POST /api/users/{userId}/roles)

4. **Use Pre-request Script**:
   ```javascript
   // Auto-set token from previous login response
   if (pm.response.code === 200) {
       var jsonData = pm.response.json();
       pm.environment.set("accessToken", jsonData.accessToken);
   }
   ```

---

## Webhook Events (Future)

Planned webhook support for:

- `user.registered` - New user registration
- `user.login` - User login
- `user.role_assigned` - Role assignment
- `user.locked` - Account locked
- `user.unlocked` - Account unlocked

---

## API Versioning (Future)

Planned versioning approach:

```
/api/v1/auth/login
/api/v2/auth/login
```

Current version is implicitly v1.

---

## Deprecation Policy

- Deprecated endpoints will return `Deprecation` header
- 6-month notice before removal
- Version headers will indicate status

---

*For more information, see README.md and QUICKSTART.md*
