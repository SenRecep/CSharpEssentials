using CSharpEssentials.Interfaces;

namespace CSharpEssentials.Entity;
/// <summary>
/// Represents a soft deletable entity base.
/// </summary>
public abstract class SoftDeletableEntityBase : EntityBase, ISoftDeletableEntityBase
{
    private DateTimeOffset? _deletedAt;
    private string? _deletedBy;
    private bool _isDeleted;
    private bool _isHardDeleted;

    public DateTimeOffset? DeletedAt => _deletedAt;

    public string? DeletedBy => _deletedBy;

    public bool IsDeleted => _isDeleted;

    public bool IsHardDeleted => _isHardDeleted;

    public void MarkAsDeleted(DateTimeOffset deletedAt, string deletedBy)
    {
        _deletedAt = deletedAt;
        _deletedBy = deletedBy;
        _isDeleted = true;
    }

    public void MarkAsHardDeleted() => _isHardDeleted = true;

    public void Restore()
    {
        _deletedAt = null;
        _deletedBy = null;
        _isDeleted = false;
        _isHardDeleted = false;
    }
}
