# FleetPulse Architecture

## Overview

FleetPulse is a full stack vehicle telemetry application designed to demonstrate production oriented software engineering practices using ASP.NET Core, React, Entity Framework Core, and SQLite.

The application allows users to:

- Submit telemetry events
- View the latest telemetry event for a vehicle
- Review historical telemetry data
- View speeding events
- Generate summary statistics

## System Architecture

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

## Backend Design

The backend follows a layered architecture.

### Controllers

Controllers are responsible for:

- Receiving HTTP requests
- Returning HTTP responses
- Translating exceptions into HTTP status codes

Controllers do not contain business logic.

### Services

Services contain business rules and application logic.

Examples include:

- Validation
- Vehicle identifier normalization
- Summary calculations
- Data transformations

Services depend on repository interfaces rather than concrete implementations.

### Repositories

Repositories are responsible for database access.

Responsibilities include:

- Saving telemetry events
- Retrieving vehicle history
- Retrieving speeding events
- Retrieving summary data

Repositories isolate Entity Framework Core from the rest of the application.

### Entity Framework Core

Entity Framework Core is used as the ORM.

Benefits include:

- Database migrations
- LINQ support
- Strongly typed queries
- Database provider flexibility

### Database

SQLite was selected because:

- Minimal infrastructure requirements
- Easy local development
- Fast setup for a take home assessment
- Easily replaceable with SQL Server or PostgreSQL

## Frontend Design

The frontend is built using React and Vite.

### Dashboard

The Dashboard page serves as the application entry point.

Responsibilities include:

- Managing page state
- Coordinating API calls
- Passing data to child components

### VehicleSearch

Responsible for:

- Vehicle ID input
- Search execution

### VehicleSummary

Responsible for:

- Displaying summary metrics
- Presenting aggregate vehicle statistics

### TelemetryTable

Responsible for:

- Displaying historical telemetry records
- Rendering tabular data

## Testing Strategy

Testing focuses on business logic and application behavior.

### Unit Tests

Service layer tests validate:

- Input validation
- Summary calculations
- Vehicle normalization
- Business rules

### Repository Tests

Repository tests validate:

- Persistence behavior
- Query correctness
- Data filtering

### API Tests

API tests validate:

- HTTP status codes
- Request handling
- Endpoint behavior

## Design Principles

The project was designed around:

- Separation of concerns
- Dependency Injection
- Single Responsibility Principle
- Testability
- Maintainability
- Simplicity over unnecessary complexity

The goal was to produce a solution that is easy to understand, modify, and extend while remaining production oriented.