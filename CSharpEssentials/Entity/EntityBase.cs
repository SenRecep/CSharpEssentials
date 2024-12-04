using CSharpEssentials.Interfaces;

namespace CSharpEssentials.Entity;

/// <summary>
/// Represents an entity base.
/// </summary>
public abstract class EntityBase : IEntityBase
{
    private DateTimeOffset _createdAt;
    private string? _createdBy;
    private DateTimeOffset? _updatedAt;
    private string? _updatedBy;
    private readonly List<IDomainEvent> _domainEvents = [];


    public DateTimeOffset CreatedAt => _createdAt;
    public string? CreatedBy => _createdBy;

    public DateTimeOffset? UpdatedAt => _updatedAt;

    public string? UpdatedBy => _updatedBy;


    public IReadOnlyList<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents() => _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void SetCreatedInfo(DateTimeOffset createdAt, string createdBy) =>
        (_createdAt, _createdBy) = (createdAt, createdBy);
    public void SetUpdatedInfo(DateTimeOffset updatedAt, string updatedBy) =>
        (_updatedAt, _updatedBy) = (updatedAt, updatedBy);
}
