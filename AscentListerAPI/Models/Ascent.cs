namespace AscentListerAPI.Models;

public class Ascent
{
    public int AscentId { get; set; }
    public int RouteId { get; set; }
    public DateOnly Date { get; set; }
    public string Style { get; set; }
    public int Attempts { get; set; }
    public string Comments { get; set; }
    public StatusEnum Status { get; set; }
}