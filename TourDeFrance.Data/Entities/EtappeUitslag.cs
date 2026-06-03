namespace TourDeFrance.Data.Entities;

public class EtappeUitslag
{
    public int Id { get; set; }

    public int EtappeId { get; set; }
    public Etappe Etappe { get; set; } = null!;

    public int RennerId { get; set; }
    public Renner Renner { get; set; } = null!;

    public int Positie { get; set; }
}
