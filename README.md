# JWTServer

A JWT server using CQRS for scalability, use this instead of built-in JWT functions (i.e. AWS Cognito) to remain cloud-agnostic.

Run, in the root directory:
```
docker-compose up --build
```
to run in debugging mode.


## Projects:

- JwtTokenServer - by default runs on *:5001, issues JWT tokens.
- JwtQueryServer - by default runs on *:5002, validates JWT tokens and retrieves claims from valid JWT tokens.
- JwtUtilties - some common utililities and base classes.
- JwtServer.Tests - XUnit test harness for the server API.

## Postman Examples:

Note for all requests to any of the server, the following header will need to be used in debugging mode:
```
Content-Type: application/json
PublicAuthToken: PublicSecret
```

### JwtTokenServer URL

> localhost:5001/Generate/

### Actions
#### GET
See a simple "hello" message.

#### POST

Get a JWT token returned with Body:
```json
[
  {"claimType":"emailAddress", "claimValue":"jackula@83.com"},
  {"claimType":"userName", "claimValue":"jackula83"},
]
```

### JwtQueryServer URLs

For token verification:
> localhost:5002/Verify

For claims retrieval:
> localhost:5002/Claims

### Actions
#### GET
See a simple "hello" message.

#### POST

Verify: Get a true (valid) or false (invalid) returned with Body:
> "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"

Claims: Get a list of claims returned with Body:
> "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"

(these are example JWT tokens, use the Generate endpoint to get the token, otherwise Verify will return Invalid and Claims will return null)

## Enviornment Variables

A full list of environment variables can be found in:
```c#
JwtUtilities.Helpers.EnvironmentVariables
```
