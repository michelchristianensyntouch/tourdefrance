using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data.Entities;

namespace TourDeFrance.Data;

public class TourDeFranceDbContext(DbContextOptions<TourDeFranceDbContext> options) : DbContext(options)
{
    public DbSet<Ploeg> Ploegen => Set<Ploeg>();
    public DbSet<Renner> Renners => Set<Renner>();
    public DbSet<Deelnemer> Deelnemers => Set<Deelnemer>();
    public DbSet<DeelnemerSelectie> DeelnemerSelecties => Set<DeelnemerSelectie>();
    public DbSet<Etappe> Etappes => Set<Etappe>();
    public DbSet<EtappeUitslag> EtappeUitslagen => Set<EtappeUitslag>();
    public DbSet<EindKlassement> EindKlassementen => Set<EindKlassement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Renner>()
            .HasIndex(r => r.Startnummer)
            .IsUnique();

        modelBuilder.Entity<DeelnemerSelectie>()
            .HasIndex(s => new { s.DeelnemerId, s.RennerId })
            .IsUnique();

        modelBuilder.Entity<EtappeUitslag>()
            .HasIndex(u => new { u.EtappeId, u.RennerId })
            .IsUnique();

        modelBuilder.Entity<EindKlassement>()
            .HasIndex(k => k.RennerId)
            .IsUnique();

        modelBuilder.Entity<Renner>()
            .HasOne(r => r.Ploeg)
            .WithMany(p => p.Renners)
            .HasForeignKey(r => r.PloegId);

        modelBuilder.Entity<DeelnemerSelectie>()
            .HasOne(s => s.Deelnemer)
            .WithMany(d => d.Selecties)
            .HasForeignKey(s => s.DeelnemerId);

        modelBuilder.Entity<DeelnemerSelectie>()
            .HasOne(s => s.Renner)
            .WithMany(r => r.Selecties)
            .HasForeignKey(s => s.RennerId);

        modelBuilder.Entity<EtappeUitslag>()
            .HasOne(u => u.Etappe)
            .WithMany(e => e.Uitslagen)
            .HasForeignKey(u => u.EtappeId);

        modelBuilder.Entity<EtappeUitslag>()
            .HasOne(u => u.Renner)
            .WithMany(r => r.EtappeUitslagen)
            .HasForeignKey(u => u.RennerId);

        modelBuilder.Entity<EindKlassement>()
            .HasOne(k => k.Renner)
            .WithOne(r => r.EindKlassement)
            .HasForeignKey<EindKlassement>(k => k.RennerId);
    }
}
