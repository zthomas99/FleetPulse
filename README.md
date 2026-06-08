# FleetPulse

![FleetPulse Dashboard](docs/screenshots/dashboard.png)

FleetPulse is a full stack vehicle telemetry application built using ASP.NET Core, React, Entity Framework Core, and SQLite.

The application allows users to submit telemetry events, retrieve vehicle activity, analyze historical data, and generate summary statistics.

## Features

### Telemetry Event Management

- Submit telemetry events
- Retrieve latest telemetry event
- Retrieve vehicle history
- Retrieve speeding events
- Generate vehicle summary statistics

### Validation

The application validates:

- Vehicle identifier
- Latitude range
- Longitude range
- Speed values
- Engine status

### Testing

The project includes:

- Service layer unit tests
- Repository tests
- API behavior tests

## Technology Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- xUnit
- Moq

### Frontend

- React
- Vite
- Axios

## Architecture

React Frontend
      |
      v
ASP.NET Core Web API
      |
      v
Service Layer
      |
      v
Repository Layer
      |
      v
Entity Framework Core
      |
      v
SQLite Database

Additional architectural details can be found in:

docs/Architecture.md 

## Project Structure

FleetPulse
├── backend
│   ├── FleetPulse.Api
│   ├── FleetPulse.Core
│   ├── FleetPulse.Infrastructure
│   └── FleetPulse.Tests
│
├── frontend
│   └── fleetpulse-ui
│
└── docs
    ├── Architecture.md
    └── FutureEnhancements.md

## Backend Setup

### Prerequisites

Install:

- .NET SDK
- Entity Framework CLI

Verify installation:

dotnet --version dotnet ef --version 

### Build

From the backend folder:

dotnet build 

### Apply Database Migrations

dotnet ef database update \
  --project FleetPulse.Infrastructure \
  --startup-project FleetPulse.Api

### Run the API

dotnet run --project FleetPulse.Api 

Swagger is available at:

http://localhost:5069/swagger 

## Frontend Setup

### Install Dependencies

From:

frontend/fleetpulse-ui 

run:

npm install 

### Run the Frontend

npm run dev 

Vite will provide the local URL.

Typically:

http://localhost:5173 

## API Endpoints

### Create Telemetry Event

POST /api/Telemetry 

Example Request:

{
  "vehicleId": "V123",
  "timestamp": "2026-06-08T12:00:00Z",
  "latitude": 33.749,
  "longitude": -84.388,
  "speedMph": 72,
  "engineStatus": "On"
}

### Get Latest Event

GET /api/Telemetry/vehicles/{vehicleId}/latest 

### Get Vehicle History

GET /api/Telemetry/vehicles/{vehicleId}/history 

### Get Speeding Events

GET /api/Telemetry/vehicles/{vehicleId}/speeding?threshold=70 

### Get Vehicle Summary

GET /api/Telemetry/vehicles/{vehicleId}/summary 

## Running Tests

From the backend folder:

dotnet test 

Tests cover:

- Validation rules
- Summary calculations
- Vehicle normalization
- Business logic
- Repository behavior

## Design Decisions

### Repository Pattern

Repositories abstract database access from the service layer.

Benefits:

- Improved testability
- Separation of concerns
- Easier maintenance

### Service Layer

Business logic is isolated from controllers.

Benefits:

- Easier testing
- Cleaner controllers
- Better maintainability

### SQLite

SQLite was selected because:

- Minimal setup requirements
- Easy local development
- Suitable for take home assessments
- Easily replaceable with SQL Server or PostgreSQL

### Vehicle Identifier Normalization

Vehicle identifiers are normalized to uppercase.

Benefits:

- Consistent lookups
- Case insensitive searches
- Improved data quality

## Future Enhancements

Potential enhancements are documented in:

docs/FutureEnhancements.md 

Examples include:

- Authentication
- Pagination
- Mapping integration
- Vehicle discovery
- Cloud deployment
- CI/CD pipelines
- Caching
- Monitoring

## Author

Created as a demonstration of full stack software engineering practices including:

- REST API development
- Frontend development
- Database design
- Automated testing
- Dependency Injection
- Layered architecture
- Production oriented design principles
