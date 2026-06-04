namespace AscentListerAPI.Models;

/// <summary>
/// A climbing route at a <see cref="Location"/>.
/// </summary>
public class Route
{
    /// <summary>Primary key.</summary>
    public int RouteId { get; set; }

    /// <summary>The name of the route.</summary>
    public string RouteName { get; set; }

    /// <summary>The route's grade, e.g. "6a" or "6b+".</summary>
    public string Grade { get; set; }

    /// <summary>An optional secondary grade for routes with a contested or combined grade.</summary>
    public string? GradeTwo { get; set; }

    /// <summary>The location (crag/area) the route belongs to.</summary>
    public Location Location { get; set; }

    /// <summary>Lifecycle status used when syncing with the client.</summary>
    public StatusEnum RouteStatus { get; set; }
}
