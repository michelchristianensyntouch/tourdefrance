using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data;
using TourDeFrance.Data.Entities;

namespace TourDeFrance.Web.Services;

public record DeelnemerScore(
    int DeelnemerId,
    string DeelnemerNaam,
    int DagPunten,
    int BonusPunten,
    int TotaalPunten
);

public record EtappeDeelnemerScore(
    int DeelnemerId,
    string DeelnemerNaam,
    int PuntenDezeEtappe,
    int CumulatiefTotaal
);

public class PuntenService(TourDeFranceDbContext db)
{
    private static readonly int[] DagPunten = [15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1];
    private static readonly int[] AKBonusPunten = [75, 70, 65, 60, 55, 50, 45, 40, 35, 30, 25, 20, 15, 10, 5];
    private static readonly int[] TruiBonusPunten = [25, 20, 15, 10, 5];

    public async Task<List<EtappeDeelnemerScore>> BerekenEtappeScoresAsync(int etappeId)
    {
        var uitslagen = await db.EtappeUitslagen
            .Where(u => u.EtappeId == etappeId && u.Positie >= 1 && u.Positie <= 15)
            .ToListAsync();

        var deelnemers = await db.Deelnemers
            .Include(d => d.Selecties)
            .ToListAsync();

        var scorePerDeelnemer = new Dictionary<int, int>();

        foreach (var deelnemer in deelnemers)
        {
            var rennerIds = deelnemer.Selecties.Select(s => s.RennerId).ToHashSet();
            var punten = uitslagen
                .Where(u => rennerIds.Contains(u.RennerId))
                .Sum(u => DagPunten[u.Positie - 1]);
            scorePerDeelnemer[deelnemer.Id] = punten;
        }

        // Bereken cumulatief totaal (alle etappes t/m nu)
        var allEtappes = await db.Etappes
            .Where(e => e.Id <= etappeId)
            .Select(e => e.Id)
            .ToListAsync();

        var alleUitslagen = await db.EtappeUitslagen
            .Where(u => allEtappes.Contains(u.EtappeId) && u.Positie >= 1 && u.Positie <= 15)
            .ToListAsync();

        var cumulatief = new Dictionary<int, int>();
        foreach (var deelnemer in deelnemers)
        {
            var rennerIds = deelnemer.Selecties.Select(s => s.RennerId).ToHashSet();
            cumulatief[deelnemer.Id] = alleUitslagen
                .Where(u => rennerIds.Contains(u.RennerId))
                .Sum(u => DagPunten[u.Positie - 1]);
        }

        return deelnemers
            .Select(d => new EtappeDeelnemerScore(
                d.Id, d.Naam,
                scorePerDeelnemer.GetValueOrDefault(d.Id),
                cumulatief.GetValueOrDefault(d.Id)))
            .OrderByDescending(s => s.PuntenDezeEtappe)
            .ToList();
    }

    public async Task<List<DeelnemerScore>> BerekenTotaalKlassementAsync()
    {
        var deelnemers = await db.Deelnemers
            .Include(d => d.Selecties)
            .ToListAsync();

        var alleUitslagen = await db.EtappeUitslagen
            .Where(u => u.Positie >= 1 && u.Positie <= 15)
            .ToListAsync();

        var eindKlassementen = await db.EindKlassementen.ToListAsync();

        var scores = new List<DeelnemerScore>();

        foreach (var deelnemer in deelnemers)
        {
            var rennerIds = deelnemer.Selecties.Select(s => s.RennerId).ToHashSet();

            var dagPunten = alleUitslagen
                .Where(u => rennerIds.Contains(u.RennerId))
                .Sum(u => DagPunten[u.Positie - 1]);

            var bonus = 0;
            foreach (var k in eindKlassementen)
            {
                if (!rennerIds.Contains(k.RennerId)) continue;

                if (k.AKPositie.HasValue && k.AKPositie <= 15)
                    bonus += AKBonusPunten[k.AKPositie.Value - 1];

                if (k.PuntenPositie.HasValue && k.PuntenPositie <= 5)
                    bonus += TruiBonusPunten[k.PuntenPositie.Value - 1];

                if (k.BergPositie.HasValue && k.BergPositie <= 5)
                    bonus += TruiBonusPunten[k.BergPositie.Value - 1];
            }

            scores.Add(new DeelnemerScore(deelnemer.Id, deelnemer.Naam, dagPunten, bonus, dagPunten + bonus));
        }

        return scores.OrderByDescending(s => s.TotaalPunten).ToList();
    }
}
