# 🍽️ Restaurant Management API

Backend for a Restaurant Management System built with **.NET 10** and **Clean Architecture**. This project demonstrates modern software engineering practices, including CQRS, MediatR, and automated testing.

## 🏗️ Architecture

The project follows the **Clean Architecture** (Onion Architecture) pattern to ensure high maintainability, testability, and independence from external frameworks.

### Backend Structure
*   **`Domain/`**: The core of the application. Contains enterprise logic, Entities (`Restaurant`, `Dish`, `User`), Enums, Constants (`UserRoles`, `PolicyNames`), and Core Interfaces. It has zero dependencies on other layers.
*   **`Application/`**: Implements business logic using the **CQRS** pattern with **MediatR**. Contains Commands, Queries, DTOs, Validators (FluentValidation), and AutoMapper profiles. 
*   **`Infrastructure/`**: Handles technical concerns such as Database Persistence (**Entity Framework Core** with PostgreSQL), Identity logic, Authorization handlers, and external services like **MinIO/S3** for object storage.
*   **`API/`**: The entry point of the application. Contains Controllers, Middlewares (Error Handling, Request Logging), and Swagger configurations.

## ✨ Key Features

-   **Restaurant Management**: Full CRUD operations for restaurants including advanced filtering, searching, and pagination.
-   **Menu & Dishes**: Manage dishes for each restaurant with automated price and description tracking.
-   **User Identity & Security**: Integrated **ASP.NET Core Identity** with JWT-based authentication and role-based authorization (Admin, Owner, User).
-   **Logo Upload**: Integration with **MinIO** or S3-compatible storage for uploading restaurant logos.
-   **Policy-based Authorization**: Custom requirements (e.g., minimum age, multiple restaurant creation limits) and resource-based authorization.
-   **Performance Testing**: Pre-configured **k6** scripts for load and stress testing.
-   **Robust Error Handling**: Global exception middleware for consistent API responses.

## 🚀 Getting Started

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker Desktop](https://www.docker.com/) (for MinIO and DB)
- [Node.js](https://nodejs.org/) (optional, for performance tests)

### 1. Environment Configuration

**API Setup:**
1. Navigate to `src/API/`.
2. Copy `.env.example` to `.env` and configure your database connection and S3 credentials.
3. Review `appsettings.Development.json`.

### 2. Infrastructure Services
Start the required services (PostgreSQL & MinIO) using Docker:
```bash
docker-compose up -d
```

### 3. Running the Application
```bash
cd src/API
dotnet run
```
The API will be available at `https://localhost:5001`. Access **Swagger UI** at `https://localhost:5001/swagger` to explore the endpoints.

### 4. Running Tests
The project includes a comprehensive test suite (Unit, Integration, and Performance):
```bash
# Run all C# tests
dotnet test

# Run Performance tests (requires k6)
cd tests/Performance.Tests
k6 run StressTests/Restaurants/Create.js
```

## 🛠️ Technologies Used

-   **Runtime**: .NET 10
-   **Database**: PostgreSQL with Entity Framework Core
-   **Storage**: MinIO / AWS S3
-   **Patterns**: CQRS, MediatR, Repository Pattern, Unit of Work
-   **Validation**: FluentValidation
-   **Mapping**: AutoMapper
-   **Testing**: xUnit, Moq, FluentAssertions, k6
-   **Documentation**: Swagger (Swashbuckle)

---

### 📝 Project Structure Highlights
```text
src/
├── Domain/         # Entities, Interfaces, Exceptions
├── Application/    # CQRS (Commands/Queries), DTOs, Logic
├── Infrastructure/ # DB Context, Repositories, S3 Service
└── API/            # Controllers, Middlewares, Program.cs
tests/
├── API.Tests/      # Integration tests
├── Application.Tests/
└── Performance.Tests/ # k6 Load testing scripts
```
