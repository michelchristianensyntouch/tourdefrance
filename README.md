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
├── TourDeFrance.Web/              # Blazor Server webapplicatie
│   ├── Components/Pages/          # Razor-pagina's (UI)
│   ├── Services/                  # Business logica
│   └── Program.cs                 # Configuratie & startup
├── TourDeFrance.Data/             # Data-laag (SQLite migraties)
│   ├── Entities/                  # EF Core domeinmodellen
│   ├── Migrations/                # SQLite database migraties
│   ├── TourDeFranceDbContext.cs
│   └── DbSeeder.cs                # Initiële data (renners, ploegen, etappes)
├── TourDeFrance.Data.SqlServer/   # SQL Server migraties (Azure)
│   └── Migrations/                # Azure SQL Server migraties
├── TourDeFrance.Tests/            # Unittests
├── infra/                         # Azure Bicep infrastructure-as-code
│   ├── main.bicep
│   └── main.bicepparam
├── .github/workflows/             # GitHub Actions CI/CD
│   └── azure-deploy.yml
├── Dockerfile                     # Container image definitie
└── TourDeFrance.slnx              # Solution bestand
```

## Database bekijken

Gebruik [DBeaver](https://dbeaver.io/) of een andere SQLite-client:

- **Type**: SQLite
- **Pad**: `C:\dev\git\tourdefrance\TourDeFrance.Web\tourdefrance.db`

## Azure deployment

### Architectuur

De applicatie draait in Azure als een Linux Docker container op **Azure App Service**, met **Azure SQL Database** als database.

```
GitHub Actions
    │  push to master
    ▼
Build & Test → Docker build → Push to ACR → Deploy Bicep → Update App Service
                                    │
                   Azure Container Registry (ACR)
                                    │
                          Azure App Service (B1 Linux)
                                    │
                       Azure SQL Database (Standard S0)
```

### Azure resources (aangemaakt via Bicep)

| Resource | SKU | Beschrijving |
|---|---|---|
| Azure Container Registry | Basic | Docker images opslaan |
| App Service Plan | B1 Linux | Hosting platform |
| App Service | - | Blazor Server web app |
| Azure SQL Server | - | Database server |
| Azure SQL Database | Standard S0 | Applicatiedatabase |
| Application Insights | - | Monitoring & telemetrie |
| Log Analytics Workspace | PerGB2018 | Log opslag |

### Eenmalige instelling

#### 1. Azure Service Principal aanmaken

```bash
az ad sp create-for-rbac \
  --name "tourdefrance-deploy" \
  --role contributor \
  --scopes /subscriptions/<subscription-id>/resourceGroups/tourdefrance-rg \
  --json-auth
```

Sla de JSON-output op als GitHub secret `AZURE_CREDENTIALS`.

#### 2. GitHub secrets instellen

Ga naar **Settings → Secrets and variables → Actions** en voeg toe:

| Secret | Waarde |
|---|---|
| `AZURE_CREDENTIALS` | JSON-output van de service principal |
| `SQL_ADMIN_PASSWORD` | Sterk wachtwoord voor de SQL Server admin |

#### 3. Handmatig deployen (optioneel)

```bash
# Resource group aanmaken
az group create --name tourdefrance-rg --location westeurope

# Infrastructure deployen
az deployment group create \
  --resource-group tourdefrance-rg \
  --template-file infra/main.bicep \
  --parameters infra/main.bicepparam \
  --parameters sqlAdminPassword=<wachtwoord>
```

### Automatische deployment

Na elke push naar `master` of `main` start de GitHub Actions workflow automatisch:

1. **Build & Test** – Solution bouwen en unittests uitvoeren
2. **Deploy infrastructure** – Bicep deployen (idempotent)
3. **Docker build & push** – Container image naar ACR pushen
4. **App Service update** – Container image bijwerken en herstarten

### Database provider configuratie

| Omgeving | `DatabaseProvider` | Database |
|---|---|---|
| Lokaal (development) | `Sqlite` (standaard) | `tourdefrance.db` |
| Azure (production) | `SqlServer` | Azure SQL Database |

De instelling `DatabaseProvider` wordt automatisch ingesteld via de App Service configuratie.

## Technologie

- **Framework**: .NET 10 / ASP.NET Core Blazor Server
- **Database**: SQLite (lokaal) / Azure SQL Database (Azure) via Entity Framework Core 10
- **UI**: Blazor Server Components (interactief via SignalR)
- **Hosting**: Azure App Service (Linux container) / Kestrel (lokaal)
- **Infrastructure**: Azure Bicep
- **CI/CD**: GitHub Actions
