# CSharpEssentials.EntityFrameworkCore

- **NuGet Package EntityFrameworkCore**: [CSharpEssentials.EntityFrameworkCore](https://www.nuget.org/packages/CSharpEssentials.EntityFrameworkCore) [![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.EntityFrameworkCore.svg)](https://www.nuget.org/packages/CSharpEssentials.EntityFrameworkCore)

CSharpEssentials.EntityFrameworkCore is a powerful extension library for Entity Framework Core that enhances its capabilities with functional programming features and DDD-friendly patterns. It provides a robust set of utilities and base classes to streamline your data access layer while maintaining clean architecture principles.

## 🌟 Key Features

- **Base Entity Classes**: Rich set of base classes for domain entities
- **Soft Delete Support**: Built-in soft delete functionality with query filters
- **Audit Trail**: Automatic tracking of creation and modification timestamps
- **Query Filters**: Flexible and composable query filter system
- **Optimistic Concurrency**: Easy setup for version-based concurrency control
- **Extension Methods**: Useful extensions for DbContext and entity operations
- **Query Performance Monitoring**: Built-in slow query detection and logging
- **PostgreSQL Support**: First-class support for PostgreSQL with naming conventions
- **Microservices Ready**: Designed for modern microservices architecture
- **Domain Events**: Built-in support for domain events and event sourcing

## 📦 Installation

```bash
dotnet add package CSharpEssentials.EntityFrameworkCore
```

## 🚀 Core Components

### Base DbContext

The `BaseDbContext<TContext>` provides a foundation for your DbContext with built-in logging and dependency injection support:

```csharp
public class MyAppDbContext : BaseDbContext<MyAppDbContext>
{
    public MyAppDbContext(
        DbContextOptions<MyAppDbContext> options,
        IServiceScopeFactory serviceScopeFactory)
        : base(options, serviceScopeFactory)
    {
    }
}
```

### PostgreSQL Integration

Configure your DbContext for PostgreSQL with proper naming conventions:

```csharp
public static class DatabaseExtensions
{
    public static IServiceCollection AddPostgreSqlDbContext<TContext>(
        this IServiceCollection services,
        string connectionString)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention()  // Uses snake_case for database objects
                   .UseCheckConstraints();          // Enables check constraints
        });

        return services;
    }
}
```

### Entity Mapping Extensions

#### Basic Entity Mapping

```csharp
public class EntityConfiguration : IEntityTypeConfiguration<MyEntity>
{
    public void Configure(EntityTypeBuilder<MyEntity> builder)
    {
        // For entities with Guid ID
        builder.EntityBaseGuidIdMap();

        // For entities with custom ID type
        builder.EntityBaseMap<MyEntity, TId>();

        // For basic audit properties
        builder.EntityBaseMap();
    }
}
```

#### Soft Delete Support

```csharp
public class SoftDeleteEntityConfiguration : IEntityTypeConfiguration<MySoftDeleteEntity>
{
    public void Configure(EntityTypeBuilder<MySoftDeleteEntity> builder)
    {
        // For entities with Guid ID
        builder.SoftDeletableEntityBaseGuidIdMap();

        // For entities with custom ID type
        builder.SoftDeletableEntityBaseMap<MySoftDeleteEntity, TId>();

        // For basic soft delete properties
        builder.SoftDeletableEntityBaseMap();
    }
}
```

### Query Filters

#### Global Soft Delete Filter

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Automatically applies soft delete filter to all entities
    modelBuilder.ApplySoftDeleteQueryFilter();
}
```

#### Custom Query Filters

```csharp
public void Configure(EntityTypeBuilder<MyEntity> builder)
{
    builder.AddQueryFilter<MyEntity>(e => e.TenantId == CurrentTenant.Id);
}
```

### Optimistic Concurrency

```csharp
public void Configure(EntityTypeBuilder<MyEntity> builder)
{
    // Adds version tracking for optimistic concurrency
    builder.OptimisticConcurrencyVersionMap();
}
```

### DbContext Extensions

#### NoTracking Control

```csharp
// Conditionally disable tracking
var entities = await dbContext.Entities
    .AsNoTracking(shouldDisableTracking)
    .ToListAsync();
```

#### Soft Delete Operations

```csharp
// Soft delete a single entity
dbContext.HardDelete(entity);

// Soft delete multiple entities
dbContext.HardDelete(entities);
```

#### Data Migration Helper

```csharp
await dbContext.MigrateDataAsync<TEntity, TSeedData>(
    seedData,
    (entities, data) => !entities.Any(),
    item => new TEntity { /* mapping logic */ }
);

// Advanced migration with update and delete support
await dbContext.MigrateDataAsync<TEntity, TSeedData, TKey>(
    seedData,
    query => query,
    e => e.Id,
    d => d.Id,
    (entity, data) => entity.Name != data.Name,
    (entity, data) => { entity.UpdateName(data.Name); return entity; },
    data => new TEntity(data),
    hardDeleteMode: true
);
```

### Query Performance Monitoring

The `SlowQueryInterceptor` provides automatic monitoring and logging of slow-running queries. Here's how to set it up:

#### 1. Register the Interceptor

First, register the SlowQueryInterceptor in your dependency injection container:

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductDbContext(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<SlowQueryInterceptor>();

        services.AddDbContext<ProductDbContext>((sp, options) =>
        {
            ...
            options.AddInterceptors(sp.GetRequiredService<SlowQueryInterceptor>());
        });

        return services;
    }
}
```

#### 2. In DbContext Configuration

Alternatively, you can get the interceptor from the service provider in your DbContext:

```csharp
public class AppDbContext : BaseDbContext<AppDbContext>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var slowQueryInterceptor = ServiceProvider.GetRequiredService<SlowQueryInterceptor>();
        optionsBuilder.AddInterceptors(slowQueryInterceptor);
    }
}
```

The interceptor automatically logs queries that take longer than 1 second to execute, including:

- Query execution time
- SQL command text
- Parameter values
- Transaction status
- Database name
- Execution context

Example log output:

```
warn: SlowQueryInterceptor[0]
      Slow query (00:00:01.5000000): SELECT * FROM Products WHERE CategoryId = @p0, Parameters: @p0=5, Transaction: No, Database: ProductDb in ReaderExecuted
```

## 🔧 Best Practices

1. **Entity Design**

   - Use appropriate base classes for entities (`EntityBase` or `SoftDeletableEntityBase`)
   - Implement proper audit trail through base classes
   - Utilize soft delete when entities need to be preserved
   - Keep entities immutable with private setters
   - Use static factory methods for entity creation

2. **Query Performance**

   - Use `AsNoTracking` for read-only queries
   - Apply global filters judiciously
   - Consider impact of soft delete on queries
   - Use proper indexes for frequently queried fields

3. **Data Consistency**

   - Implement optimistic concurrency for concurrent updates
   - Use transaction scope for complex operations
   - Maintain audit trail for all modifications
   - Use domain events for cross-aggregate consistency

4. **Functional Approach**

   - Use immutable properties where possible
   - Implement domain events for state changes
   - Keep entities focused and cohesive
   - Use Result pattern for error handling

5. **Microservices Architecture**
   - Keep DbContext scoped to bounded contexts
   - Use domain events for cross-service communication
   - Implement proper entity isolation
   - Use proper naming conventions for database objects

## 📚 Examples

### Complete Entity Example

```csharp
public class Product : SoftDeletableEntityBase<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

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

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
        Raise(new PriceUpdatedEvent(Id, newPrice));
    }
}
```

### Microservice DbContext Configuration Example

```csharp
public class ProductDbContext : BaseDbContext<ProductDbContext>
{
    public DbSet<Product> Products { get; set; }

    public ProductDbContext(
        DbContextOptions<ProductDbContext> options,
        IServiceScopeFactory serviceScopeFactory)
        : base(options, serviceScopeFactory)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplySoftDeleteQueryFilter();

        modelBuilder.Entity<Product>(builder =>
        {
            builder.SoftDeletableEntityBaseGuidIdMap();
            builder.OptimisticConcurrencyVersionMap();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            // Add check constraints
            builder.HasCheckConstraint("CK_Product_Price", "price >= 0");
        });
    }
}
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
