namespace AscentListerAPI.Models;

/// <summary>
/// Tracks the lifecycle of an entity so the client and API can reconcile changes
/// during synchronization.
/// </summary>
public enum StatusEnum
{
    /// <summary>The entity was created on the client and not yet persisted.</summary>
    NEW,

    /// <summary>The entity already exists in the database.</summary>
    EXISTS,

    /// <summary>The entity has been marked for deletion.</summary>
    DELETED,
}
