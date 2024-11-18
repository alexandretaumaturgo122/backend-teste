namespace Backend.Core.Domain;

public interface ITimestampEntity
{
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset? UpdatedAt { get; }
}