namespace AscentListerAPI.Models;

public class Route
{
    public int RouteId { get; set; }
    public string RouteName { get; set; }
    public string Grade { get; set; }
    public int LocationId { get; set; }
    public StatusEnum RouteStatus { get; set; }
}