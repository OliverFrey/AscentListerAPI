namespace AscentListerAPI.Models;

/// <summary>
/// A climbing location (crag or area) that routes belong to.
/// </summary>
public class Location
{
    /// <summary>Primary key.</summary>
    public int LocationId  { get; set; }

    /// <summary>The name of the crag or sector.</summary>
    public string LocationName  { get; set; }

    /// <summary>The broader area the location sits within.</summary>
    public string LocationAreaName  { get; set; }

    /// <summary>The country the location is in.</summary>
    public string locationCountry { get; set; }

    /// <summary>Lifecycle status used when syncing with the client.</summary>
    public StatusEnum LocationStatus { get; set; }
}
