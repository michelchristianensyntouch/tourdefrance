# Architectuuroverzicht – Tour de France Poule

## Overzicht

De applicatie is opgebouwd als een **Blazor Server** webapplicatie met een gelaagde architectuur. Alle logica draait server-side; de browser communiceert via een **SignalR**-verbinding met de server.

```
┌─────────────────────────────────────────────────────────┐
│                        Browser                          │
│              (HTML + SignalR WebSocket)                  │
└─────────────────────────┬───────────────────────────────┘
                          │
┌─────────────────────────▼───────────────────────────────┐
│              TourDeFrance.Web (Blazor Server)            │
│                                                         │
│  ┌──────────────────────────────────────────────────┐   │
│  │              Razor Components (Pages)            │   │
│  │  Home  │  Renners  │  TeamInvoer  │  Klassement  │   │
│  │        │  EtappeUitslagen  │  AdminEtappe         │   │
│  └──────────────────┬───────────────────────────────┘   │
│                     │ DI (Scoped)                        │
│  ┌──────────────────▼───────────────────────────────┐   │
│  │                  Services                        │   │
│  │  RennerService  │  DeelnemerService               │   │
│  │  EtappeService  │  PuntenService                  │   │
│  └──────────────────┬───────────────────────────────┘   │
└─────────────────────┼───────────────────────────────────┘
                      │ EF Core (DbContext)
┌─────────────────────▼───────────────────────────────────┐
│              TourDeFrance.Data (Class Library)           │
│                                                         │
│  TourDeFranceDbContext  │  DbSeeder                      │
│                                                         │
│  Entities:                                              │
│  Ploeg │ Renner │ Etappe │ Deelnemer                    │
│  DeelnemerSelectie │ EtappeUitslag │ EindKlassement      │
└─────────────────────┬───────────────────────────────────┘
                      │ ADO.NET / SQLite
┌─────────────────────▼───────────────────────────────────┐
│                   tourdefrance.db                       │
│                   (SQLite bestand)                      │
└─────────────────────────────────────────────────────────┘
```

---

## Projecten

### `TourDeFrance.Web` – Blazor Server app
- Entry point en DI-configuratie in `Program.cs`
- Razor Components in `Components/Pages/`
- Services bevatten alle business logica
- Communiceert met de data-laag via EF Core DbContext

### `TourDeFrance.Data` – Class Library
- Bevat alle EF Core entiteiten, de DbContext en de DbSeeder
- Wordt als projectreferentie toegevoegd aan `TourDeFrance.Web`
- Migrations worden hier beheerd

### `TourDeFrance.Tests` – xUnit testproject
- Bevat unittests voor de services (met name `PuntenService`)

---

## Domeinmodel (Entiteiten)

```
Ploeg
├── Id (PK)
└── Naam

Renner
├── Id (PK)
├── Startnummer (uniek)
├── Naam
└── Ploeg

Etappe
├── Id (PK)
├── EtappeNummer (uniek)
├── Datum
├── Van
└── Naar

Deelnemer
├── Id (PK)
└── Naam

DeelnemerSelectie            ← team van een deelnemer
├── Id (PK)
├── DeelnemerId (FK)
├── RennerId (FK)
└── Volgorde (1–15; 1–3 = podiumvoorspelling)

EtappeUitslag                ← dagresultaat per renner
├── Id (PK)
├── EtappeId (FK)
├── RennerId (FK)
└── Positie (1–15)

EindKlassement               ← AK / Groene Trui / Bollentrui
├── Id (PK)
├── TruiType (AK | Groen | Bollen)
├── RennerId (FK)
└── Positie
```

---

## Services

| Service | Verantwoordelijkheid |
|---|---|
| `RennerService` | CRUD voor renners en ploegen; zoeken en filteren |
| `DeelnemerService` | Aanmaken deelnemers, opslaan/ophalen teamsamenstelling |
| `EtappeService` | Opslaan etappe-uitslagen en eindklassementen |
| `PuntenService` | Berekening van dagpunten, AK-bonuspunten en truibonuspunten |

Alle services zijn geregistreerd als **Scoped** (één instantie per Blazor circuit/verbinding), wat de standaard is voor Blazor Server met EF Core.

---

## Puntenberekenaar (`PuntenService`)

```
PuntenService
├── BerekenDeelnemerScores(etappeId)
│   └── EtappeDeelnemerScore[]
│       ├── DagPunten       (positie 1-15 → 15-1 pts)
│       └── DeelnemerNaam
│
└── BerekenTotaalScores()
    └── DeelnemerScore[]
        ├── DagPuntenTotaal
        ├── AKBonusPunten   (positie 1-15 → 75/70/.../5 pts)
        ├── TruiBonusPunten (positie 1-5  → 25/20/15/10/5 pts, per trui)
        └── TotaalPunten
```

---

## Gegevensopslag

- **Database**: SQLite (`tourdefrance.db`)
- **ORM**: Entity Framework Core 10
- **Migraties**: Code-first via `dotnet ef migrations`
- **Seeding**: `DbSeeder.cs` vult bij eerste start:
  - 22 wielerploegen (Tour de France 2023)
  - 128 renners met startnummers
  - 21 etappes met datum en route

De database wordt automatisch aangemaakt en gemigreerd bij opstart (`context.Database.MigrateAsync()`).

---

## Communicatiepatroon (Blazor Server)

```
Gebruiker klikt knop
        │
        ▼
Blazor Component (C# event handler)
        │
        ▼
Service (business logica)
        │
        ▼
EF Core DbContext → SQLite
        │
        ▼
Resultaat terug naar Component
        │
        ▼
StateHasChanged() → SignalR → DOM-update in browser
```

Alle server-round-trips verlopen via de bestaande SignalR-verbinding — er zijn geen aparte REST API-aanroepen.

---

## Opstartflow

```
Program.cs
├── AddDbContext<TourDeFranceDbContext>(UseSqlite)
├── AddScoped<RennerService>
├── AddScoped<DeelnemerService>
├── AddScoped<EtappeService>
├── AddScoped<PuntenService>
├── AddRazorComponents().AddInteractiveServerComponents()
└── app.Run()
    ├── MigrateAsync()          ← database up-to-date
    └── DbSeeder.SeedAsync()    ← basisdata indien leeg
```
