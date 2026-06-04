namespace AscentListerAPI.Models;

/// <summary>
/// A logged climb of a route on a given day.
/// </summary>
public class Ascent
{
    /// <summary>Primary key.</summary>
    public int AscentId { get; set; }

    /// <summary>The route that was climbed.</summary>
    public Route Route { get; set; }

    /// <summary>The date the ascent was made.</summary>
    public DateOnly Date { get; set; }

    /// <summary>How the route was climbed, e.g. "Flash", "Redpoint", "Onsight".</summary>
    public string Style { get; set; }

    /// <summary>The number of attempts it took to send the route.</summary>
    public int Attempts { get; set; }

    /// <summary>Optional free-text notes about the ascent.</summary>
    public string? Comments { get; set; }

    /// <summary>Lifecycle status used when syncing with the client.</summary>
    public StatusEnum Status { get; set; }
}
