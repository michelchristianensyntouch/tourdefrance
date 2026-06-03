namespace TourDeFrance.Data.Entities;

public class Deelnemer
{
    public int Id { get; set; }
    public string Naam { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime AangemaaktOp { get; set; } = DateTime.UtcNow;

    public ICollection<DeelnemerSelectie> Selecties { get; set; } = [];
}
