namespace Posts.Domain.Abstractions;

public interface IEntity<T> where T : struct
{
    T Id { get; set; }
}
