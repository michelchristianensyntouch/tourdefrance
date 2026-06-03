# Tour de France Poule Applicatie

## Overzicht
Een Blazor Server webapplicatie met SQL Server database voor het beheren van een Tour de France wielerpoule. Deelnemers stellen teams samen van 15 renners en verdienen punten op basis van dagelijkse etappe-uitslagen en eindklassementen.

## Technologie Stack
- **Frontend**: Blazor Server (.NET 8)
- **Backend**: ASP.NET Core
- **Database**: SQL Server (LocalDB voor ontwikkeling)
- **ORM**: Entity Framework Core
- **UI Framework**: Bootstrap 5 + Blazor componenten

## Project Locatie
`C:\dev\git\tourdefrance`

---

## Functionaliteiten

### Scherm 1: Renners Overzicht
- Toon alle Tour de France renners in een doorzoekbare/sorteerbare tabel
- Kolommen: Startnummer, Renner Naam, Ploegnaam
- Filteren op ploeg, zoeken op naam

### Scherm 2: Deelnemer Team Invoer
- Deelnemer voert zijn/haar naam in
- Selecteer 15 renners via startnummer
- Automatisch invullen van renner naam en ploeg bij invoer startnummer
- Eerste 3 selecties zijn de voorspelling voor het eindpodium (top 3 klassement)
- Validatie: geen dubbele renners, precies 15 selecties

### Scherm 3: Dagelijkse Etappe Uitslagen
- Bekijk punten per deelnemer per etappe
- Etappe selectie dropdown
- Tabel: Deelnemer Naam, Punten Behaald, Lopend Totaal

### Scherm 4: Algemeen Klassement
- Totale scores van alle deelnemers over alle verreden etappes
- Inclusief dagpunten + bonuspunten (na laatste etappe)
- Sorteerbaar klassement

---

## Puntensysteem

### Dagelijkse Etappe Punten
Top 15 finishers in elke etappe leveren punten op voor deelnemers die hen geselecteerd hebben:
| Positie | Punten |
|---------|--------|
| 1e      | 15     |
| 2e      | 14     |
| 3e      | 13     |
| ...     | ...    |
| 15e     | 1      |

### Eind Bonuspunten

#### Gele Trui (Algemeen Klassement)
Punten worden toegekend als renner in team van 15 zit:
| AK Positie | Punten |
|------------|--------|
| 1e         | 75     |
| 2e         | 70     |
| 3e         | 65     |
| 4e         | 60     |
| 5e         | 55     |
| 6e         | 50     |
| 7e         | 45     |
| 8e         | 40     |
| 9e         | 35     |
| 10e        | 30     |
| 11e        | 25     |
| 12e        | 20     |
| 13e        | 15     |
| 14e        | 10     |
| 15e        | 5      |

#### Groene Trui (Puntenklassement)
Punten voor top 5 als renner in team zit:
| Positie | Punten |
|---------|--------|
| 1e      | 25     |
| 2e      | 20     |
| 3e      | 15     |
| 4e      | 10     |
| 5e      | 5      |

#### Bolletjestrui (Bergklassement)
Punten voor top 5 als renner in team zit:
| Positie | Punten |
|---------|--------|
| 1e      | 25     |
| 2e      | 20     |
| 3e      | 15     |
| 4e      | 10     |
| 5e      | 5      |

---

## Database Schema

### Tabellen

#### Ploegen (Wielerploegen)
```sql
CREATE TABLE Ploegen (
    Id INT PRIMARY KEY IDENTITY,
    Naam NVARCHAR(100) NOT NULL,
    Land NVARCHAR(50)
);
```

#### Renners
```sql
CREATE TABLE Renners (
    Id INT PRIMARY KEY IDENTITY,
    Startnummer INT NOT NULL UNIQUE,
    Voornaam NVARCHAR(100) NOT NULL,
    Achternaam NVARCHAR(100) NOT NULL,
    PloegId INT FOREIGN KEY REFERENCES Ploegen(Id),
    Land NVARCHAR(50)
);
```

#### Deelnemers
```sql
CREATE TABLE Deelnemers (
    Id INT PRIMARY KEY IDENTITY,
    Naam NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255),
    AangemaaktOp DATETIME2 DEFAULT GETDATE()
);
```

#### DeelnemerSelecties
```sql
CREATE TABLE DeelnemerSelecties (
    Id INT PRIMARY KEY IDENTITY,
    DeelnemerId INT FOREIGN KEY REFERENCES Deelnemers(Id),
    RennerId INT FOREIGN KEY REFERENCES Renners(Id),
    SelectieVolgorde INT NOT NULL, -- 1-15, waarbij 1-3 podiumvoorspellingen zijn
    CONSTRAINT UQ_DeelnemerRenner UNIQUE (DeelnemerId, RennerId)
);
```

#### Etappes
```sql
CREATE TABLE Etappes (
    Id INT PRIMARY KEY IDENTITY,
    EtappeNummer INT NOT NULL,
    Datum DATE,
    StartLocatie NVARCHAR(100),
    EindLocatie NVARCHAR(100),
    Afstand DECIMAL(5,1),
    EtappeType NVARCHAR(50) -- Vlak, Berg, Tijdrit, etc.
);
```

#### EtappeUitslagen
```sql
CREATE TABLE EtappeUitslagen (
    Id INT PRIMARY KEY IDENTITY,
    EtappeId INT FOREIGN KEY REFERENCES Etappes(Id),
    RennerId INT FOREIGN KEY REFERENCES Renners(Id),
    Positie INT NOT NULL,
    CONSTRAINT UQ_EtappeRenner UNIQUE (EtappeId, RennerId)
);
```

#### EindKlassementen
```sql
CREATE TABLE EindKlassementen (
    Id INT PRIMARY KEY IDENTITY,
    RennerId INT FOREIGN KEY REFERENCES Renners(Id),
    AKPositie INT, -- Algemeen Klassement (Geel)
    PuntenPositie INT, -- Puntenklassement (Groen)
    BergPositie INT, -- Bergklassement (Bolletjes)
    CONSTRAINT UQ_RennerKlassement UNIQUE (RennerId)
);
```

---

## Project Structuur

```
TourDeFrance/
├── TourDeFrance.sln
├── TourDeFrance.Web/                    # Blazor Server App
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Components/
│   │   ├── Layout/
│   │   │   ├── MainLayout.razor
│   │   │   └── NavMenu.razor
│   │   └── Pages/
│   │       ├── Home.razor
│   │       ├── Renners.razor            # Scherm 1: Renners Overzicht
│   │       ├── TeamInvoer.razor         # Scherm 2: Team Samenstellen
│   │       ├── EtappeUitslagen.razor    # Scherm 3: Dagelijkse Uitslagen
│   │       └── Klassement.razor         # Scherm 4: Totaal Klassement
│   ├── Services/
│   │   ├── IRennerService.cs
│   │   ├── RennerService.cs
│   │   ├── IDeelnemerService.cs
│   │   ├── DeelnemerService.cs
│   │   ├── IEtappeService.cs
│   │   ├── EtappeService.cs
│   │   └── IPuntenService.cs
│   │   └── PuntenService.cs
│   └── wwwroot/
│       └── css/
├── TourDeFrance.Data/                   # Data Access Laag
│   ├── TourDeFranceDbContext.cs
│   ├── Entities/
│   │   ├── Ploeg.cs
│   │   ├── Renner.cs
│   │   ├── Deelnemer.cs
│   │   ├── DeelnemerSelectie.cs
│   │   ├── Etappe.cs
│   │   ├── EtappeUitslag.cs
│   │   └── EindKlassement.cs
│   └── Migrations/
└── TourDeFrance.Tests/                  # Unit Tests
    └── PuntenServiceTests.cs
```

---

## Implementatie Taken

### Fase 1: Project Opzet
- [ ] Maak solution en project structuur aan
- [ ] Configureer Entity Framework Core met SQL Server
- [ ] Maak database entities en DbContext
- [ ] Voer initiële migratie uit om database te creëren

### Fase 2: Data Laag
- [ ] Implementeer repository pattern of directe EF Core gebruik
- [ ] Maak seed data voor ploegen en renners (kan uit CSV geïmporteerd worden)
- [ ] Maak seed data voor etappes (21 etappes)

### Fase 3: Core Services
- [ ] Implementeer RennerService (CRUD voor renners)
- [ ] Implementeer DeelnemerService (deelnemer registratie, team selectie)
- [ ] Implementeer EtappeService (etappe uitslagen invoer)
- [ ] Implementeer PuntenService (bereken dag- en bonuspunten)

### Fase 4: UI - Renners Pagina
- [ ] Maak Renners.razor component
- [ ] Toon alle renners in sorteerbare/filterbare tabel
- [ ] Voeg zoekfunctionaliteit toe

### Fase 5: UI - Team Invoer Pagina
- [ ] Maak TeamInvoer.razor component
- [ ] Deelnemer naam invoer
- [ ] 15 renner selectie velden met auto-complete
- [ ] Validatie en opslaan functionaliteit

### Fase 6: UI - Dagelijkse Uitslagen Pagina
- [ ] Maak EtappeUitslagen.razor component
- [ ] Etappe selector
- [ ] Toon deelnemer punten per etappe
- [ ] Admin: Etappe uitslagen invoeren

### Fase 7: UI - Klassement Pagina
- [ ] Maak Klassement.razor component
- [ ] Bereken en toon totaal punten
- [ ] Inclusief bonuspunten na laatste etappe

### Fase 8: Admin Functies
- [ ] Etappe uitslagen invoer interface
- [ ] Eindklassementen invoer
- [ ] Data import/export (renners CSV)

### Fase 9: Testen & Afwerking
- [ ] Unit tests voor punten logica
- [ ] Integratie tests
- [ ] UI afwerking en responsive design

---

## Opmerkingen
- De applicatie gebruikt Blazor Server voor real-time updates
- SQL Server LocalDB voor ontwikkeling, kan geconfigureerd worden voor volledige SQL Server in productie
- Overweeg authenticatie toe te voegen indien nodig voor deelnemerbeheer
- Toekomstige uitbreiding: Live data integratie van wielren API's
