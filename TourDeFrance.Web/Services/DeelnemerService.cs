using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data;
using TourDeFrance.Data.Entities;

namespace TourDeFrance.Web.Services;

public class DeelnemerService(TourDeFranceDbContext db)
{
    public async Task<List<Deelnemer>> GetAlleDeelnemersAsync() =>
        await db.Deelnemers
            .OrderBy(d => d.Naam)
            .ToListAsync();

    public async Task<Deelnemer?> GetDeelnemerMetSelectiesAsync(int id) =>
        await db.Deelnemers
            .Include(d => d.Selecties)
            .ThenInclude(s => s.Renner)
            .ThenInclude(r => r.Ploeg)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<bool> DeelnemerBestaatAsync(string naam) =>
        await db.Deelnemers.AnyAsync(d => d.Naam.ToLower() == naam.ToLower());

    public async Task<Deelnemer> MaakDeelnemerAanAsync(string naam, string? email = null)
    {
        var deelnemer = new Deelnemer { Naam = naam, Email = email };
        db.Deelnemers.Add(deelnemer);
        await db.SaveChangesAsync();
        return deelnemer;
    }

    public async Task SlaTeamOpAsync(int deelnemerId, List<int> startnummers)
    {
        // Verwijder bestaande selecties
        var bestaande = db.DeelnemerSelecties.Where(s => s.DeelnemerId == deelnemerId);
        db.DeelnemerSelecties.RemoveRange(bestaande);

        var renners = await db.Renners
            .Where(r => startnummers.Contains(r.Startnummer))
            .ToListAsync();

        var selecties = startnummers
            .Select((nr, idx) =>
            {
                var renner = renners.FirstOrDefault(r => r.Startnummer == nr);
                return renner == null ? null : new DeelnemerSelectie
                {
                    DeelnemerId = deelnemerId,
                    RennerId = renner.Id,
                    Volgorde = idx + 1
                };
            })
            .Where(s => s != null)
            .Cast<DeelnemerSelectie>()
            .ToList();

        db.DeelnemerSelecties.AddRange(selecties);
        await db.SaveChangesAsync();
    }
}
