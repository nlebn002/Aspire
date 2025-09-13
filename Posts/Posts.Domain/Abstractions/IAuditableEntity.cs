namespace Posts.Domain.Abstractions;

public interface IAuditableEntity<T> : IEntity<T> where T : struct
{
    DateTime CreatedAtUtc { get; set; }
    DateTime? UpdatedAtUtc { get; set; }
}