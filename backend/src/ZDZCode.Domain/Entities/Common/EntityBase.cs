namespace ZDZCode.Domain.Entities.Common;

public abstract class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public bool Active { get; private set; } = true;
}
