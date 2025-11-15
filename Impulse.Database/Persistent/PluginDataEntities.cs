namespace Impulse.Repository.Persistent;

using System;

internal sealed class StoredSingletonModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string TypeName { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTime UpdatedOn { get; set; }
        = DateTime.UtcNow;
}

internal sealed class StoredCollectionModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string TypeName { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public int Position { get; set; }
        = 0;

    public DateTime CreatedOn { get; set; }
        = DateTime.UtcNow;
}
