# EngageGov API

A modern, production-ready REST API for civic engagement built with .NET 10 and C#, following Clean Architecture principles and industry best practices.

## 🏗️ Architecture

This project implements **Clean Architecture** (also known as Onion Architecture or Hexagonal Architecture), which provides:

- **Separation of Concerns**: Each layer has distinct responsibilities
- **Dependency Inversion**: Dependencies point inward, with business logic at the core
- **Testability**: Business logic is independent of frameworks and infrastructure
- **Maintainability**: Clear structure makes the codebase easier to understand and modify
- **Scalability**: Architecture supports growth and changing requirements

### Project Structure

```
EngageGov/
├── src/
│   ├── EngageGov.Domain/              # Core business logic and entities
│   │   ├── Entities/                  # Domain entities (Citizen, Proposal, etc.)
│   │   ├── Enums/                     # Domain enumerations
│   │   ├── Interfaces/                # Repository interfaces (DIP)
│   │   └── Common/                    # Base classes and shared domain logic
│   │
│   ├── EngageGov.Application/         # Application business rules
│   │   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Interfaces/                # Service interfaces
│   │   ├── Services/                  # Business logic implementation
│   │   └── UseCases/                  # Application use cases
│   │
│   ├── EngageGov.Infrastructure/      # External concerns
│   │   ├── Data/                      # Database context and configurations
│   │   ├── Repositories/              # Repository implementations
│   │   └── DependencyInjection.cs     # Infrastructure DI configuration
│   │
│   └── EngageGov.API/                 # Presentation layer
│       ├── Controllers/               # REST API controllers
│       ├── Middleware/                # Custom middleware
│       └── Program.cs                 # Application entry point
│
└── tests/
    ├── EngageGov.UnitTests/          # Unit tests for domain and application layers
    └── EngageGov.IntegrationTests/   # Integration tests for the API
```

## 🎯 Design Principles

This project adheres to **SOLID** principles and clean code practices:

### Single Responsibility Principle (SRP)
- Each class has one reason to change
- Services handle specific business operations
- Repositories manage data access for specific entities

### Open/Closed Principle (OCP)
- Classes are open for extension but closed for modification
- Strategy pattern used where appropriate
- Interfaces define contracts for extensibility

### Liskov Substitution Principle (LSP)
- Derived classes can substitute their base classes
- Repository pattern allows swapping implementations

### Interface Segregation Principle (ISP)
- Specific interfaces rather than one general-purpose interface
- `ICitizenRepository` and `IProposalRepository` have targeted methods

### Dependency Inversion Principle (DIP)
- High-level modules don't depend on low-level modules
- Both depend on abstractions (interfaces)
- Domain layer defines interfaces, Infrastructure implements them

## 🔐 Security Features

- **HTTPS Enforcement**: All traffic encrypted
- **CORS Policy**: Configurable cross-origin resource sharing
- **Security Headers**: XSS protection, content type options, frame options
- **Global Exception Handling**: Prevents information leakage in production
- **Input Validation**: Data annotations and business rule validation
- **JWT Ready**: Infrastructure prepared for JWT authentication
- **SQL Injection Protection**: Entity Framework Core with parameterized queries

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK or later
- (Optional) SQL Server for production database
- (Optional) Visual Studio 2022, VS Code, or JetBrains Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/lucas-explica/engage-gov-api.git
   cd engage-gov-api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the tests**
   ```bash
   dotnet test
   ```

5. **Run the API**
   ```bash
   cd src/EngageGov.API
   dotnet run
   ```

6. **Access Swagger UI**
   - Navigate to: `https://localhost:5001` (or the port shown in terminal)
   - The Swagger UI will be displayed at the root URL in development mode

## 📊 Database

### Development
The API uses an **in-memory database** by default, which is perfect for development and testing. No setup required!

### Production
To use SQL Server in production:

1. Update `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=EngageGov;Trusted_Connection=True;"
     }
   }
   ```

2. The application will automatically use SQL Server when a connection string is provided.

## 🧪 Testing

### Unit Tests
Tests for domain entities and application services:
```bash
dotnet test --filter FullyQualifiedName~UnitTests
```

### Integration Tests
End-to-end API tests:
```bash
dotnet test --filter FullyQualifiedName~IntegrationTests
```

### Test Coverage
The project includes comprehensive tests for:
- Domain entity validation and business rules
- Application service logic
- Repository implementations
- API endpoints (integration tests)

## 📡 API Endpoints

### Citizens
- `GET /api/citizens` - Get all citizens
- `GET /api/citizens/{id}` - Get citizen by ID
- `GET /api/citizens/email/{email}` - Get citizen by email
- `POST /api/citizens` - Create new citizen
- `DELETE /api/citizens/{id}` - Delete citizen

### Proposals
- `GET /api/proposals` - Get all proposals
- `GET /api/proposals/{id}` - Get proposal by ID
- `GET /api/proposals/status/{status}` - Get proposals by status
- `GET /api/proposals/citizen/{citizenId}` - Get proposals by citizen
- `GET /api/proposals/search?searchTerm={term}` - Search proposals
- `POST /api/proposals?citizenId={id}` - Create new proposal
- `PUT /api/proposals/{id}` - Update proposal
- `DELETE /api/proposals/{id}` - Delete proposal

### Health Check
- `GET /health` - API health status

## 🛠️ Technologies & Frameworks

- **.NET 10**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 10**: ORM for database access
- **Swashbuckle**: OpenAPI/Swagger documentation
- **xUnit**: Testing framework
- **Moq**: Mocking framework for tests
- **FluentAssertions**: Fluent test assertion library

## 📝 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""  // Leave empty for in-memory database
  },
  "AllowedOrigins": [
    "http://localhost:3000",
    "http://localhost:4200"
  ],
  "JwtSettings": {
    "SecretKey": "your-secret-key-change-in-production",
    "Issuer": "EngageGov",
    "Audience": "EngageGov-API",
    "ExpirationInMinutes": 60
  }
}
```

## 🔄 Development Workflow

1. **Create feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make changes following the architecture**
   - Domain: Add entities, value objects, or business rules
   - Application: Add DTOs, services, or use cases
   - Infrastructure: Add repositories or external service integrations
   - API: Add controllers or middleware

3. **Write tests**
   - Unit tests for domain and application logic
   - Integration tests for API endpoints

4. **Run quality checks**
   ```bash
   dotnet build
   dotnet test
   ```

5. **Commit and push**
   ```bash
   git add .
   git commit -m "Description of changes"
   git push origin feature/your-feature-name
   ```

## 📚 Additional Resources

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core)

## 📄 License

This project is open source and available under the MIT License.

## 👥 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📞 Contact

For questions or support, please open an issue in the GitHub repository.