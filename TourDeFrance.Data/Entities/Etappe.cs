namespace TourDeFrance.Data.Entities;

public class Etappe
{
    public int Id { get; set; }
    public int EtappeNummer { get; set; }
    public DateOnly? Datum { get; set; }
    public string? StartLocatie { get; set; }
    public string? EindLocatie { get; set; }
    public decimal? Afstand { get; set; }
    public string? EtappeType { get; set; }

    public ICollection<EtappeUitslag> Uitslagen { get; set; } = [];
}
