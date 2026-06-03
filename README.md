# 🚴 Tour de France Poule

Een webapplicatie voor een Tour de France fantasy cycling league ("poule"), gebouwd met .NET 10 Blazor Server en SQLite.

## Functionaliteit

| Pagina | Beschrijving |
|---|---|
| **Home** | Welkomstpagina met overzicht van regels en navigatie |
| **Renners** | Volledig overzicht van alle renners met ploeg en startnummer, doorzoekbaar en filterbaar |
| **Mijn Team** | Stel een team samen van 15 renners op startnummer; de eerste 3 zijn je podiumvoorspelling |
| **Etappe Uitslagen** | Dagelijkse puntentelling per deelnemer per etappe |
| **Klassement** | Totaalstand van alle deelnemers inclusief bonuspunten |
| **Admin** | Voer etappe-uitslagen en eindklassementen in |

## Puntensysteem

### Dagpunten (elke etappe)
Per etappe scoor je punten voor elke renner uit je team die in de top 15 finisht:

| Positie | Punten |
|---|---|
| 1e | 15 punten |
| 2e | 14 punten |
| … | … |
| 15e | 1 punt |

### Bonuspunten (na de Tour)

**Algemeen Klassement (Gele Trui)** – voor de top 15 eindklassement:

| Positie | Punten |
|---|---|
| 1e | 75 punten |
| 2e | 70 punten |
| … | … |
| 15e | 5 punten |

**Groene Trui & Bollentrui** – voor de top 5 eindklassement:

| Positie | Punten |
|---|---|
| 1e | 25 punten |
| 2e | 20 punten |
| 3e | 15 punten |
| 4e | 10 punten |
| 5e | 5 punten |

## Installatie & Uitvoeren

### Vereisten
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Starten

```bash
cd TourDeFrance.Web
dotnet run
```

De app is beschikbaar op: **http://localhost:5201**

De SQLite-database (`tourdefrance.db`) wordt automatisch aangemaakt en gevuld met renners, ploegen en etappes bij de eerste start.

### Alternatieve poort

```bash
dotnet run --urls "http://localhost:5000"
```

## Projectstructuur

```
tourdefrance/
├── TourDeFrance.Web/        # Blazor Server webapplicatie
│   ├── Components/Pages/    # Razor-pagina's (UI)
│   ├── Services/            # Business logica
│   └── Program.cs           # Configuratie & startup
├── TourDeFrance.Data/       # Data-laag
│   ├── Entities/            # EF Core domeinmodellen
│   ├── Migrations/          # Database migraties
│   ├── TourDeFranceDbContext.cs
│   └── DbSeeder.cs          # Initiële data (renners, ploegen, etappes)
├── TourDeFrance.Tests/      # Unittests
└── TourDeFrance.slnx        # Solution bestand
```

## Database bekijken

Gebruik [DBeaver](https://dbeaver.io/) of een andere SQLite-client:

- **Type**: SQLite
- **Pad**: `C:\dev\git\tourdefrance\TourDeFrance.Web\tourdefrance.db`

## Technologie

- **Framework**: .NET 10 / ASP.NET Core Blazor Server
- **Database**: SQLite via Entity Framework Core 10
- **UI**: Blazor Server Components (interactief via SignalR)
- **Hosting**: Zelf-gehost (Kestrel)
