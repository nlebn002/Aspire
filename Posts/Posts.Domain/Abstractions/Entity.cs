namespace Posts.Domain.Abstractions;

public abstract class Entity<T> : IAuditableEntity<T> where T : struct
{
    public T Id { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
