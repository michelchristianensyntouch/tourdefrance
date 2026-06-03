namespace TourDeFrance.Data.Entities;

public class Ploeg
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public string? Land { get; set; }

    public ICollection<Renner> Renners { get; set; } = [];
}
