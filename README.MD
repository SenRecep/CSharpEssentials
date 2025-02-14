# CSharpEssentials

CSharpEssentials is a comprehensive library that enhances C#'s functional programming capabilities. It provides a robust set of tools and utilities designed to make your C# applications more maintainable, testable, and aligned with functional programming principles.

## 🌟 Package Information

- **NuGet Package**: [CSharpEssentials](https://www.nuget.org/packages/CSharpEssentials) [![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.svg)](https://www.nuget.org/packages/CSharpEssentials) [![NuGet](https://img.shields.io/nuget/dt/CSharpEssentials.svg)](https://www.nuget.org/packages/CSharpEssentials)
- **NuGet Package EntityFrameworkCore**: [CSharpEssentials.EntityFrameworkCore](https://www.nuget.org/packages/CSharpEssentials.EntityFrameworkCore) [![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.EntityFrameworkCore.svg)](https://www.nuget.org/packages/CSharpEssentials.EntityFrameworkCore) [![NuGet](https://img.shields.io/nuget/dt/CSharpEssentials.EntityFrameworkCore.svg)](https://www.nuget.org/packages/CSharpEssentials.EntityFrameworkCore)
- **NuGet Package AspNetCore**: [CSharpEssentials.AspNetCore](https://www.nuget.org/packages/CSharpEssentials.AspNetCore) [![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.AspNetCore.svg)](https://www.nuget.org/packages/CSharpEssentials.AspNetCore) [![NuGet](https://img.shields.io/nuget/dt/CSharpEssentials.AspNetCore.svg)](https://www.nuget.org/packages/CSharpEssentials.AspNetCore)
- **NuGet Package RequestResponseLogging**: [CSharpEssentials.RequestResponseLogging](https://www.nuget.org/packages/CSharpEssentials.RequestResponseLogging) [![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.RequestResponseLogging.svg)](https://www.nuget.org/packages/CSharpEssentials.RequestResponseLogging) [![NuGet](https://img.shields.io/nuget/dt/CSharpEssentials.RequestResponseLogging.svg)](https://www.nuget.org/packages/CSharpEssentials.RequestResponseLogging)
- **NuGet Package GcpSecretManager**: [CSharpEssentials.GcpSecretManager](https://www.nuget.org/packages/CSharpEssentials.GcpSecretManager) [![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.GcpSecretManager.svg)](https://www.nuget.org/packages/CSharpEssentials.GcpSecretManager) [![NuGet](https://img.shields.io/nuget/dt/CSharpEssentials.GcpSecretManager.svg)](https://www.nuget.org/packages/CSharpEssentials.GcpSecretManager)
- **GitHub Repository**: [SenRecep/CSharpEssentials](https://github.com/SenRecep/CSharpEssentials) [![GitHub stars](https://img.shields.io/github/stars/SenRecep/CSharpEssentials.svg)](https://github.com/SenRecep/CSharpEssentials/stargazers)
- **Author**: [Recep Şen](https://www.nuget.org/profiles/recepsen)

## 🌟 Key Features

- **Functional Programming Support**: Enhanced functional programming capabilities for C#
- **Type Safety**: Strong type safety across all modules
- **Error Handling**: Comprehensive error handling with Result pattern
- **Domain-Driven Design**: Support for DDD patterns and practices
- **Performance**: Optimized for high performance with modern C# features
- **Testing**: Built with testability in mind

## 📦 Core Modules

### Maybe Monad

[`Maybe<T>`](CSharpEssentials/Monad/Readme.MD) - Safe handling of nullable values:

```csharp
Maybe<string> someValue = Maybe.From("Hello");
Maybe<string> noValue = Maybe.None;

// LINQ support
var result = someValue
    .Select(str => str.ToUpper())
    .Where(str => str.Length > 5);
```

### Result Pattern

[`Result<T>`](CSharpEssentials/Results/Readme.MD) - Functional approach to error handling:

```csharp
public Result<User> CreateUser(UserDto dto)
{
    if (string.IsNullOrEmpty(dto.Email))
        return Error.Validation("EMAIL_REQUIRED", "Email is required");

    var user = new User(dto);
    return Result<User>.Success(user);
}
```

### Rule Engine

[`RuleEngine`](CSharpEssentials/Rules/Readme.MD) - Complex business rule management:

```csharp
public class EmailValidationRule : IRule<string>
{
    public Result Evaluate(string email, CancellationToken cancellationToken = default)
    {
        return email.Contains("@")
            ? Result.Success()
            : Error.Validation("INVALID_EMAIL", "Email must contain @");
    }
}
```

### Error Types

[`Error`](CSharpEssentials/Errors/Readme.MD) - Structured error handling:

```csharp
var error = Error.Validation(
    code: "USER_INVALID_AGE",
    description: "User age must be greater than 18"
);
```

### Discriminated Unions

[`Any<T0,T1,...>`](CSharpEssentials/Any/Readme.MD) - Type-safe union types:

```csharp
public Any<string, int> ParseOrKeepAsString(string input)
{
    return int.TryParse(input, out int number)
        ? number
        : input;
}
```

### Entity Framework Support

[`EntityFrameworkCore`](CSharpEssentials.EntityFrameworkCore/Readme.MD) - Enhanced EF Core support with functional patterns:

```csharp
public class Product : SoftDeletableEntityBase<Guid>
{
    private Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public static Product Create(string name, decimal price)
    {
        var product = new Product(name, price);
        product.Raise(new ProductCreatedEvent(product.Id));
        return product;
    }
}

// Configure in DbContext
public void Configure(EntityTypeBuilder<Product> builder)
{
    builder.SoftDeletableEntityBaseGuidIdMap();
    builder.OptimisticConcurrencyVersionMap();
}
```

### Time Management

[`DateTimeProvider`](CSharpEssentials/Time/Readme.MD) - Testable datetime handling:

```csharp
public class OrderService
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public Order CreateOrder()
    {
        return new Order { CreatedAt = _dateTimeProvider.UtcNow };
    }
}
```

### JSON Utilities

[`JSON`](CSharpEssentials/Json/Readme.MD) - Enhanced JSON capabilities:

```csharp
var options = EnhancedJsonSerializerOptions.DefaultOptions;
string json = myObject.ConvertToJson(options);
```

### Extensions

[`Extensions`](CSharpEssentials/Extensions/Readme.MD) - Useful extension methods:

```csharp
// String transformations
"helloWorld".ToPascalCase();  // => "HelloWorld"
"hello_world".ToCamelCase();  // => "helloWorld"

// Collection operations
var randomItem = list.GetRandomItem();
```

### Request Response Logging

[`RequestResponseLogging`](CSharpEssentials.RequestResponseLogging/Readme.MD) - Comprehensive HTTP traffic monitoring:

```csharp
public void Configure(IApplicationBuilder app)
{
    app.AddRequestResponseLogging(opt =>
    {
        // Configure logging options
        var loggingOptions = LoggingOptions.CreateAllFields();
        loggingOptions.HeaderKeys.Add("X-Correlation-Id");

        // Use the configured logger
        opt.UseLogger(app.Services.GetRequiredService<ILoggerFactory>(), loggingOptions);
    });
}
```

### ASP.NET Core Integration

[`AspNetCore`](CSharpEssentials.AspNetCore/Readme.MD) - Enhanced ASP.NET Core capabilities:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddExceptionHandler<GlobalExceptionHandler>()
        .ConfigureModelValidatorResponse()
        .ConfigureSystemTextJson()
        .AddEnhancedProblemDetails()
        .AddAndConfigureApiVersioning()
        .AddSwagger<DefaultConfigureSwaggerOptions>(
            SecuritySchemes.JwtBearerTokenSecurity,
            Assembly.GetExecutingAssembly()
        );
}

public void Configure(IApplicationBuilder app)
{
    app.UseVersionableSwagger();
    app.UseExceptionHandler();
    app.UseStatusCodePages();
}
```

### Google Cloud Secret Manager Integration

[`GcpSecretManager`](CSharpEssentials.GcpSecretManager/Readme.MD) - Seamless integration with Google Cloud Secret Manager:

```csharp
// Basic configuration in Program.cs or Startup.cs
builder.Configuration.AddGcpSecretManager();

// Advanced configuration with options
builder.Configuration.AddGcpSecretManager(options =>
{
    options.CredentialsPath = "/path/to/credentials.json";
    options.AddProject(new ProjectSecretConfiguration
    {
        ProjectId = "your-gcp-project-id",
        Region = "europe-west1",
        PrefixFilters = ["app_", "service_"],
        SecretIds = ["specific-secret1"],
        RawSecretIds = ["config-json"],      // Skip JSON parsing for these secrets
        RawSecretPrefixes = ["raw_"]         // Skip JSON parsing for secrets with these prefixes
    });
});

// Configuration in appsettings.json
{
  "GoogleSecretManager": {
    "Projects": [
      {
        "ProjectId": "your-gcp-project-id",
        "Region": "europe-west1",
        "PrefixFilters": ["app_"],
        "SecretIds": ["specific-secret1"],
        "RawSecretIds": ["config-json"],
        "RawSecretPrefixes": ["raw_"]
      }
    ]
  }
}
```

Key features:
- Seamless integration with .NET configuration system
- Region-specific secret management
- Flexible secret filtering with prefix and exact matches
- JSON value flattening with control over parsing behavior
- Batch processing for optimal performance
- Automatic secret rotation support
- Built-in retry policies for resilience

### Secret Management

- Use region-specific endpoints for better latency
- Implement proper credentials management
- Use prefix filters to limit secret access
- Configure appropriate batch sizes
- Monitor secret loading performance
- Implement proper error handling
- Use JSON flattening for complex values with control over parsing behavior
- Configure proper retry policies
- Use RawSecretIds and RawSecretPrefixes to control JSON parsing behavior

## 🚀 Getting Started

### Installation

Choose the packages you need:

#### Core Package

[![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.svg)](https://www.nuget.org/packages/CSharpEssentials)

```bash
dotnet add package CSharpEssentials
```

#### Entity Framework Core Integration

[![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.EntityFrameworkCore.svg)](https://www.nuget.org/packages/CSharpEssentials.EntityFrameworkCore)

```bash
dotnet add package CSharpEssentials.EntityFrameworkCore
```

#### ASP.NET Core Integration

[![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.AspNetCore.svg)](https://www.nuget.org/packages/CSharpEssentials.AspNetCore)

```bash
dotnet add package CSharpEssentials.AspNetCore
```

#### Request Response Logging

[![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.RequestResponseLogging.svg)](https://www.nuget.org/packages/CSharpEssentials.RequestResponseLogging)

```bash
dotnet add package CSharpEssentials.RequestResponseLogging
```

#### Google Cloud Secret Manager Integration

[![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.GcpSecretManager.svg)](https://www.nuget.org/packages/CSharpEssentials.GcpSecretManager)

```bash
dotnet add package CSharpEssentials.GcpSecretManager
```

You can also install the packages directly from the [NuGet Gallery](https://www.nuget.org/profiles/recepsen).

### Basic Setup

Import the required namespaces based on your needs:

```csharp
// Core functionality
using CSharpEssentials;

// Entity Framework Core integration
using CSharpEssentials.EntityFrameworkCore;

// ASP.NET Core integration
using CSharpEssentials.AspNetCore;

// Request Response Logging
using CSharpEssentials.RequestResponseLogging;
```

## 📚 Documentation

### Core Modules Documentation

#### Functional Programming Essentials

- [Maybe Monad](CSharpEssentials/Monad/Readme.MD) - Null-safe programming with Maybe pattern
- [Result Pattern](CSharpEssentials/Results/Readme.MD) - Railway-oriented programming with Result types
- [Any Type (Discriminated Unions)](CSharpEssentials/Any/Readme.MD) - Type-safe union types for C#
- [Extensions](CSharpEssentials/Extensions/Readme.MD) - Functional programming extension methods

#### Domain Modeling

- [Rule Engine](CSharpEssentials/Rules/Readme.MD) - Business rule validation and processing
- [Error Types](CSharpEssentials/Errors/Readme.MD) - Structured error handling system
- [Entity Base](CSharpEssentials/Entity/Readme.MD) - Base classes and patterns for domain entities

#### Infrastructure

- [JSON Utilities](CSharpEssentials/Json/Readme.MD) - Enhanced JSON serialization and handling
- [Time Management](CSharpEssentials/Time/Readme.MD) - Testable datetime operations

### Integration Modules Documentation

#### Web Development

- [ASP.NET Core Integration](CSharpEssentials.AspNetCore/Readme.MD)
  - Global exception handling
  - API versioning
  - Swagger integration
  - Problem Details support
  - Model validation
  - Security configurations

#### Data Access

- [Entity Framework Core Integration](CSharpEssentials.EntityFrameworkCore/Readme.MD)
  - Soft delete support
  - Audit logging
  - Query interceptors
  - Domain events
  - Optimistic concurrency
  - Entity configurations

#### Logging and Monitoring

- [Request Response Logging](CSharpEssentials.RequestResponseLogging/Readme.MD)
  - HTTP traffic monitoring
  - Request/Response logging
  - Performance tracking
  - Custom log writers

#### Secret Management

- [Google Cloud Secret Manager Integration](CSharpEssentials.GcpSecretManager/Readme.MD)
  - Configuration provider integration
  - Region-specific secret management
  - Flexible secret filtering
  - Batch processing support
  - JSON value flattening
  - Automatic secret rotation
  - Built-in retry policies

## 🔧 Best Practices

### Error Handling

- Use `Result<T>` for operations that can fail
- Prefer `Maybe<T>` over null values for optional values
- Use structured `Error` types for domain errors
- Chain operations with LINQ-style methods on `Result<T>`
- Handle validation errors separately from domain errors
- Use `Error.Validation()` for input validation failures
- Use `Error.NotFound()` for resource not found scenarios
- Use `Error.Unauthorized()` for authentication/authorization failures

### Domain Modeling

- Extend `EntityBase` for domain entities
- Use `RuleEngine` for complex validations
- Implement domain events for state changes
- Keep entities immutable where possible
- Use static factory methods for entity creation
- Encapsulate business rules within the domain model
- Use value objects for complex value types
- Implement `IRule<T>` for reusable business rules

### Functional Programming

- Use `Any<T0,T1>` for type-safe union types
- Leverage extension methods for fluent interfaces
- Use pure functions where possible
- Avoid side effects in business logic
- Use immutable collections when appropriate
- Chain operations using LINQ and functional extensions
- Use pattern matching with discriminated unions

### ASP.NET Core Integration

- Use global exception handling middleware
- Configure proper API versioning
- Implement proper request/response logging
- Use problem details for error responses
- Configure Swagger documentation properly
- Use proper security headers
- Implement proper model validation
- Use proper HTTP status codes

### Entity Framework Core

- Use `SoftDeletableEntityBase` for soft delete support
- Configure proper entity type configurations
- Use optimistic concurrency where needed
- Implement proper audit logging
- Use proper indexing strategies
- Configure proper lazy loading settings
- Use proper query tracking behavior
- Monitor slow queries with interceptors

### Testing

- Use `DateTimeProvider` for time-dependent tests
- Mock interfaces instead of concrete implementations
- Utilize `Result<T>` for predictable error scenarios
- Write unit tests for business rules
- Test error scenarios thoroughly
- Use proper test data builders
- Implement integration tests for critical paths
- Test validation rules independently

### Performance

- Use proper caching strategies
- Implement proper database indexing
- Use async/await properly
- Avoid N+1 query problems
- Use proper connection pooling
- Implement proper response compression
- Monitor and log performance metrics
- Use proper batch operations

### Secret Management

- Use region-specific endpoints for better latency
- Implement proper credentials management
- Use prefix filters to limit secret access
- Configure appropriate batch sizes
- Monitor secret loading performance
- Implement proper error handling
- Use JSON flattening for complex values with control over parsing behavior
- Configure proper retry policies
- Use RawSecretIds and RawSecretPrefixes to control JSON parsing behavior

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

This library was inspired by and builds upon the work of several excellent open-source projects:

- [ErrorOr](https://github.com/amantinband/error-or) - A simple, fluent discriminated union of an error or a result
- [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) - Functional extensions for C#
- [OneOf](https://github.com/mcintyre321/OneOf) - Easy to use F#-like discriminated unions for C#
- [TechBuddySolution](https://github.com/salihcantekin/TechBuddySolution) - The foundation for our Request Response Logging module

Special thanks to all contributors who have helped shape this library and to the maintainers of these inspiring projects.

## 📬 Support

For support, please open an issue in the GitHub repository or contact the maintainers.

## 🔍 Related Projects

Take a look at [SenRecep.Aspire](https://www.nuget.org/packages/SenRecep.Aspire), a modern microservices template built with .NET Aspire 9.0. The template provides a robust foundation for building scalable, maintainable, and cloud-ready microservices applications. Check out the [GitHub repository](https://github.com/SenRecep/Aspire) for more details.

## Quick Start

1. Add the following configuration to your `appsettings.json`:

```json
{
  "GoogleSecretManager": {
    "Projects": [
      {
        // Required: Your Google Cloud project ID
        "ProjectId": "your-gcp-project-id",
        
        // Optional: Specific region for better latency (e.g., "europe-west1", "us-central1")
        "Region": "europe-west1",
        
        // Optional: Only load secrets with these prefixes
        "PrefixFilters": ["app_", "service_"],
        
        // Optional: Load only these specific secrets
        "SecretIds": ["database-password", "api-key"],
        
        // Optional: Skip JSON parsing for these specific secrets
        "RawSecretIds": ["config-json", "plain-text"],
        
        // Optional: Skip JSON parsing for secrets with these prefixes
        "RawSecretPrefixes": ["raw_", "text_"]
      }
    ]
  }
}
```

2. Configure the secret manager in your application:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add GCP Secret Manager configuration
builder.Configuration.AddGcpSecretManager();
```
