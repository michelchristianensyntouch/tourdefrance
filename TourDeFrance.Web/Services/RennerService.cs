using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data;
using TourDeFrance.Data.Entities;

namespace TourDeFrance.Web.Services;

public class RennerService(TourDeFranceDbContext db)
{
    public async Task<List<Renner>> GetAlleRennersAsync(string? zoekterm = null, int? ploegId = null)
    {
        var query = db.Renners
            .Include(r => r.Ploeg)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(zoekterm))
        {
            zoekterm = zoekterm.ToLower();
            query = query.Where(r =>
                r.Voornaam.ToLower().Contains(zoekterm) ||
                r.Achternaam.ToLower().Contains(zoekterm) ||
                r.Startnummer.ToString().Contains(zoekterm));
        }

        if (ploegId.HasValue)
            query = query.Where(r => r.PloegId == ploegId.Value);

        return await query.OrderBy(r => r.Startnummer).ToListAsync();
    }

    public async Task<Renner?> GetRennerByStartnummerAsync(int startnummer) =>
        await db.Renners.Include(r => r.Ploeg)
            .FirstOrDefaultAsync(r => r.Startnummer == startnummer);

    public async Task<List<Ploeg>> GetAllePloegen() =>
        await db.Ploegen.OrderBy(p => p.Naam).ToListAsync();
}
