namespace TourDeFrance.Data.Entities;

public class EindKlassement
{
    public int Id { get; set; }

    public int RennerId { get; set; }
    public Renner Renner { get; set; } = null!;

    /// <summary>Algemeen klassement (gele trui)</summary>
    public int? AKPositie { get; set; }

    /// <summary>Puntenklassement (groene trui)</summary>
    public int? PuntenPositie { get; set; }

    /// <summary>Bergklassement (bolletjestrui)</summary>
    public int? BergPositie { get; set; }
}
