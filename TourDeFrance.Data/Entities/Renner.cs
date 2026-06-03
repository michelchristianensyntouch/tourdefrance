namespace TourDeFrance.Data.Entities;

public class Renner
{
    public int Id { get; set; }
    public int Startnummer { get; set; }
    public string Voornaam { get; set; } = string.Empty;
    public string Achternaam { get; set; } = string.Empty;
    public string? Land { get; set; }

    public int PloegId { get; set; }
    public Ploeg Ploeg { get; set; } = null!;

    public ICollection<DeelnemerSelectie> Selecties { get; set; } = [];
    public ICollection<EtappeUitslag> EtappeUitslagen { get; set; } = [];
    public EindKlassement? EindKlassement { get; set; }

    public string VolledigeNaam => $"{Voornaam} {Achternaam}";
}
