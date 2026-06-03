namespace TourDeFrance.Data.Entities;

public class DeelnemerSelectie
{
    public int Id { get; set; }

    public int DeelnemerId { get; set; }
    public Deelnemer Deelnemer { get; set; } = null!;

    public int RennerId { get; set; }
    public Renner Renner { get; set; } = null!;

    /// <summary>Volgorde 1-15; positie 1-3 zijn podiumvoorspellingen.</summary>
    public int Volgorde { get; set; }
}
