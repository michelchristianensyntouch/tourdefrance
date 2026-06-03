using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data;
using TourDeFrance.Data.Entities;

namespace TourDeFrance.Web.Services;

public class EtappeService(TourDeFranceDbContext db)
{
    public async Task<List<Etappe>> GetAlleEtappesAsync() =>
        await db.Etappes.OrderBy(e => e.EtappeNummer).ToListAsync();

    public async Task<Etappe?> GetEtappeAsync(int id) =>
        await db.Etappes
            .Include(e => e.Uitslagen).ThenInclude(u => u.Renner).ThenInclude(r => r.Ploeg)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<List<EtappeUitslag>> GetUitslagenVoorEtappeAsync(int etappeId) =>
        await db.EtappeUitslagen
            .Include(u => u.Renner).ThenInclude(r => r.Ploeg)
            .Where(u => u.EtappeId == etappeId)
            .OrderBy(u => u.Positie)
            .ToListAsync();

    public async Task SlaUitslagenOpAsync(int etappeId, List<(int RennerId, int Positie)> uitslagen)
    {
        var bestaande = db.EtappeUitslagen.Where(u => u.EtappeId == etappeId);
        db.EtappeUitslagen.RemoveRange(bestaande);

        var nieuweUitslagen = uitslagen.Select(u => new EtappeUitslag
        {
            EtappeId = etappeId,
            RennerId = u.RennerId,
            Positie = u.Positie
        });

        db.EtappeUitslagen.AddRange(nieuweUitslagen);
        await db.SaveChangesAsync();
    }

    public async Task<List<EtappeUitslag>> GetEtappeUitslagenAsync(int etappeId) =>
        await db.EtappeUitslagen
            .Include(u => u.Renner).ThenInclude(r => r.Ploeg)
            .Where(u => u.EtappeId == etappeId)
            .OrderBy(u => u.Positie)
            .ToListAsync();

    public async Task SlaEtappeUitslagOpAsync(int etappeId, Dictionary<int, int> positieNaarRennerId)
    {
        var uitslagen = positieNaarRennerId.Select(kv => (RennerId: kv.Value, Positie: kv.Key)).ToList();
        await SlaUitslagenOpAsync(etappeId, uitslagen);
    }

    public async Task SlaEindKlassementOpAsync(
        Dictionary<int, int> akPositieNaarRennerId,
        Dictionary<int, int> groenPositieNaarRennerId,
        Dictionary<int, int> bolletjesPositieNaarRennerId)
    {
        var ak = akPositieNaarRennerId.Select(kv => (RennerId: kv.Value, AKPositie: kv.Key)).ToList();
        var groen = groenPositieNaarRennerId.Select(kv => (RennerId: kv.Value, PuntenPositie: kv.Key)).ToList();
        var berg = bolletjesPositieNaarRennerId.Select(kv => (RennerId: kv.Value, BergPositie: kv.Key)).ToList();
        await SlaEindKlassementInternOpAsync(ak, groen, berg);
    }


    private async Task SlaEindKlassementInternOpAsync(
        List<(int RennerId, int AKPositie)> ak,
        List<(int RennerId, int PuntenPositie)> groen,
        List<(int RennerId, int BergPositie)> berg)
    {
        db.EindKlassementen.RemoveRange(db.EindKlassementen);

        var klassementen = new Dictionary<int, EindKlassement>();

        foreach (var (rennerId, pos) in ak)
        {
            if (!klassementen.TryGetValue(rennerId, out var k))
                klassementen[rennerId] = k = new EindKlassement { RennerId = rennerId };
            k.AKPositie = pos;
        }
        foreach (var (rennerId, pos) in groen)
        {
            if (!klassementen.TryGetValue(rennerId, out var k))
                klassementen[rennerId] = k = new EindKlassement { RennerId = rennerId };
            k.PuntenPositie = pos;
        }
        foreach (var (rennerId, pos) in berg)
        {
            if (!klassementen.TryGetValue(rennerId, out var k))
                klassementen[rennerId] = k = new EindKlassement { RennerId = rennerId };
            k.BergPositie = pos;
        }

        db.EindKlassementen.AddRange(klassementen.Values);
        await db.SaveChangesAsync();
    }
}
