using Microsoft.EntityFrameworkCore;
using TourDeFrance.Data.Entities;

namespace TourDeFrance.Data;

public static class DbSeeder
{
    public static void Seed(TourDeFranceDbContext context)
    {
        if (context.Ploegen.Any()) return;

        var ploegen = new List<Ploeg>
        {
            new() { Naam = "UAE Team Emirates", Land = "VAE" },
            new() { Naam = "INEOS Grenadiers", Land = "Groot-Brittannië" },
            new() { Naam = "Jumbo-Visma", Land = "Nederland" },
            new() { Naam = "Soudal Quick-Step", Land = "België" },
            new() { Naam = "EF Education-EasyPost", Land = "VS" },
            new() { Naam = "Trek-Segafredo", Land = "VS" },
            new() { Naam = "Bahrain Victorious", Land = "Bahrein" },
            new() { Naam = "Bora-Hansgrohe", Land = "Duitsland" },
            new() { Naam = "Movistar Team", Land = "Spanje" },
            new() { Naam = "AG2R Citroën", Land = "Frankrijk" },
            new() { Naam = "Astana Qazaqstan", Land = "Kazachstan" },
            new() { Naam = "Cofidis", Land = "Frankrijk" },
            new() { Naam = "DSM-Firmenich", Land = "Nederland" },
            new() { Naam = "Groupama-FDJ", Land = "Frankrijk" },
            new() { Naam = "Intermarché-Circus", Land = "België" },
            new() { Naam = "Israel-Premier Tech", Land = "Israël" },
            new() { Naam = "Lotto Dstny", Land = "België" },
            new() { Naam = "Alpecin-Deceuninck", Land = "België" },
            new() { Naam = "TotalEnergies", Land = "Frankrijk" },
            new() { Naam = "Arkéa-B&B Hotels", Land = "Frankrijk" },
            new() { Naam = "Uno-X Pro Cycling", Land = "Noorwegen" },
            new() { Naam = "Lidl-Trek", Land = "VS" },
        };

        context.Ploegen.AddRange(ploegen);
        context.SaveChanges();

        var renners = new List<Renner>
        {
            // UAE Team Emirates (startnummers 1-8)
            new() { Startnummer = 1, Voornaam = "Tadej", Achternaam = "Pogačar", Land = "Slovenië", PloegId = ploegen[0].Id },
            new() { Startnummer = 2, Voornaam = "Adam", Achternaam = "Yates", Land = "Groot-Brittannië", PloegId = ploegen[0].Id },
            new() { Startnummer = 3, Voornaam = "Marc", Achternaam = "Soler", Land = "Spanje", PloegId = ploegen[0].Id },
            new() { Startnummer = 4, Voornaam = "João", Achternaam = "Almeida", Land = "Portugal", PloegId = ploegen[0].Id },
            new() { Startnummer = 5, Voornaam = "Tim", Achternaam = "Wellens", Land = "België", PloegId = ploegen[0].Id },
            new() { Startnummer = 6, Voornaam = "Nils", Achternaam = "Politt", Land = "Duitsland", PloegId = ploegen[0].Id },
            new() { Startnummer = 7, Voornaam = "Jay", Achternaam = "Vine", Land = "Australië", PloegId = ploegen[0].Id },
            new() { Startnummer = 8, Voornaam = "Brandon", Achternaam = "McNulty", Land = "VS", PloegId = ploegen[0].Id },
            // INEOS Grenadiers (11-18)
            new() { Startnummer = 11, Voornaam = "Carlos", Achternaam = "Rodríguez", Land = "Spanje", PloegId = ploegen[1].Id },
            new() { Startnummer = 12, Voornaam = "Geraint", Achternaam = "Thomas", Land = "Groot-Brittannië", PloegId = ploegen[1].Id },
            new() { Startnummer = 13, Voornaam = "Tom", Achternaam = "Pidcock", Land = "Groot-Brittannië", PloegId = ploegen[1].Id },
            new() { Startnummer = 14, Voornaam = "Egan", Achternaam = "Bernal", Land = "Colombia", PloegId = ploegen[1].Id },
            new() { Startnummer = 15, Voornaam = "Michal", Achternaam = "Kwiatkowski", Land = "Polen", PloegId = ploegen[1].Id },
            new() { Startnummer = 16, Voornaam = "Luke", Achternaam = "Rowe", Land = "Groot-Brittannië", PloegId = ploegen[1].Id },
            new() { Startnummer = 17, Voornaam = "Ben", Achternaam = "Turner", Land = "Groot-Brittannië", PloegId = ploegen[1].Id },
            new() { Startnummer = 18, Voornaam = "Jonathan", Achternaam = "Castroviejo", Land = "Spanje", PloegId = ploegen[1].Id },
            // Jumbo-Visma (21-28)
            new() { Startnummer = 21, Voornaam = "Jonas", Achternaam = "Vingegaard", Land = "Denemarken", PloegId = ploegen[2].Id },
            new() { Startnummer = 22, Voornaam = "Wout", Achternaam = "van Aert", Land = "België", PloegId = ploegen[2].Id },
            new() { Startnummer = 23, Voornaam = "Sepp", Achternaam = "Kuss", Land = "VS", PloegId = ploegen[2].Id },
            new() { Startnummer = 24, Voornaam = "Tiesj", Achternaam = "Benoot", Land = "België", PloegId = ploegen[2].Id },
            new() { Startnummer = 25, Voornaam = "Wilco", Achternaam = "Kelderman", Land = "Nederland", PloegId = ploegen[2].Id },
            new() { Startnummer = 26, Voornaam = "Christophe", Achternaam = "Laporte", Land = "Frankrijk", PloegId = ploegen[2].Id },
            new() { Startnummer = 27, Voornaam = "Dylan", Achternaam = "van Baarle", Land = "Nederland", PloegId = ploegen[2].Id },
            new() { Startnummer = 28, Voornaam = "Nathan", Achternaam = "Van Hooydonck", Land = "België", PloegId = ploegen[2].Id },
            // Soudal Quick-Step (31-38)
            new() { Startnummer = 31, Voornaam = "Remco", Achternaam = "Evenepoel", Land = "België", PloegId = ploegen[3].Id },
            new() { Startnummer = 32, Voornaam = "Julian", Achternaam = "Alaphilippe", Land = "Frankrijk", PloegId = ploegen[3].Id },
            new() { Startnummer = 33, Voornaam = "Kasper", Achternaam = "Asgreen", Land = "Denemarken", PloegId = ploegen[3].Id },
            new() { Startnummer = 34, Voornaam = "Pieter", Achternaam = "Serry", Land = "België", PloegId = ploegen[3].Id },
            new() { Startnummer = 35, Voornaam = "Ilan", Achternaam = "Van Wilder", Land = "België", PloegId = ploegen[3].Id },
            new() { Startnummer = 36, Voornaam = "Zdeněk", Achternaam = "Štybar", Land = "Tsjechië", PloegId = ploegen[3].Id },
            new() { Startnummer = 37, Voornaam = "Louis", Achternaam = "Vervaeke", Land = "België", PloegId = ploegen[3].Id },
            new() { Startnummer = 38, Voornaam = "Yves", Achternaam = "Lampaert", Land = "België", PloegId = ploegen[3].Id },
            // EF Education (41-48)
            new() { Startnummer = 41, Voornaam = "Rigoberto", Achternaam = "Urán", Land = "Colombia", PloegId = ploegen[4].Id },
            new() { Startnummer = 42, Voornaam = "Simon", Achternaam = "Carr", Land = "Groot-Brittannië", PloegId = ploegen[4].Id },
            new() { Startnummer = 43, Voornaam = "Neilson", Achternaam = "Powless", Land = "VS", PloegId = ploegen[4].Id },
            new() { Startnummer = 44, Voornaam = "Owain", Achternaam = "Doull", Land = "Groot-Brittannië", PloegId = ploegen[4].Id },
            new() { Startnummer = 45, Voornaam = "Stefan", Achternaam = "Bissegger", Land = "Zwitserland", PloegId = ploegen[4].Id },
            new() { Startnummer = 46, Voornaam = "Alberto", Achternaam = "Bettiol", Land = "Italië", PloegId = ploegen[4].Id },
            new() { Startnummer = 47, Voornaam = "Lenny", Achternaam = "Martinez", Land = "Frankrijk", PloegId = ploegen[4].Id },
            new() { Startnummer = 48, Voornaam = "Esteban", Achternaam = "Chaves", Land = "Colombia", PloegId = ploegen[4].Id },
            // Trek-Segafredo (51-58)
            new() { Startnummer = 51, Voornaam = "Mads", Achternaam = "Pedersen", Land = "Denemarken", PloegId = ploegen[5].Id },
            new() { Startnummer = 52, Voornaam = "Mattias", Achternaam = "Skjelmose", Land = "Denemarken", PloegId = ploegen[5].Id },
            new() { Startnummer = 53, Voornaam = "Quinn", Achternaam = "Simmons", Land = "VS", PloegId = ploegen[5].Id },
            new() { Startnummer = 54, Voornaam = "Jasper", Achternaam = "Stuyven", Land = "België", PloegId = ploegen[5].Id },
            new() { Startnummer = 55, Voornaam = "Giulio", Achternaam = "Ciccone", Land = "Italië", PloegId = ploegen[5].Id },
            new() { Startnummer = 56, Voornaam = "Alex", Achternaam = "Kirsch", Land = "Luxemburg", PloegId = ploegen[5].Id },
            new() { Startnummer = 57, Voornaam = "Toms", Achternaam = "Skujiņš", Land = "Letland", PloegId = ploegen[5].Id },
            new() { Startnummer = 58, Voornaam = "Kenny", Achternaam = "Elissonde", Land = "Frankrijk", PloegId = ploegen[5].Id },
            // Bahrain Victorious (61-68)
            new() { Startnummer = 61, Voornaam = "Mikel", Achternaam = "Landa", Land = "Spanje", PloegId = ploegen[6].Id },
            new() { Startnummer = 62, Voornaam = "Pello", Achternaam = "Bilbao", Land = "Spanje", PloegId = ploegen[6].Id },
            new() { Startnummer = 63, Voornaam = "Matej", Achternaam = "Mohorič", Land = "Slovenië", PloegId = ploegen[6].Id },
            new() { Startnummer = 64, Voornaam = "Phil", Achternaam = "Bauhaus", Land = "Duitsland", PloegId = ploegen[6].Id },
            new() { Startnummer = 65, Voornaam = "Damiano", Achternaam = "Caruso", Land = "Italië", PloegId = ploegen[6].Id },
            new() { Startnummer = 66, Voornaam = "Jack", Achternaam = "Haig", Land = "Australië", PloegId = ploegen[6].Id },
            new() { Startnummer = 67, Voornaam = "Gino", Achternaam = "Mäder", Land = "Zwitserland", PloegId = ploegen[6].Id },
            new() { Startnummer = 68, Voornaam = "Fred", Achternaam = "Wright", Land = "Groot-Brittannië", PloegId = ploegen[6].Id },
            // Bora-Hansgrohe (71-78)
            new() { Startnummer = 71, Voornaam = "Primož", Achternaam = "Roglič", Land = "Slovenië", PloegId = ploegen[7].Id },
            new() { Startnummer = 72, Voornaam = "Aleksandr", Achternaam = "Vlasov", Land = "Rusland", PloegId = ploegen[7].Id },
            new() { Startnummer = 73, Voornaam = "Bob", Achternaam = "Jungels", Land = "Luxemburg", PloegId = ploegen[7].Id },
            new() { Startnummer = 74, Voornaam = "Lennard", Achternaam = "Kämna", Land = "Duitsland", PloegId = ploegen[7].Id },
            new() { Startnummer = 75, Voornaam = "Nils", Achternaam = "Politt", Land = "Duitsland", PloegId = ploegen[7].Id },
            new() { Startnummer = 76, Voornaam = "Sam", Achternaam = "Oomen", Land = "Nederland", PloegId = ploegen[7].Id },
            new() { Startnummer = 77, Voornaam = "Emanuel", Achternaam = "Buchmann", Land = "Duitsland", PloegId = ploegen[7].Id },
            new() { Startnummer = 78, Voornaam = "Patrick", Achternaam = "Gamper", Land = "Zwitserland", PloegId = ploegen[7].Id },
            // Movistar (81-88)
            new() { Startnummer = 81, Voornaam = "Enric", Achternaam = "Mas", Land = "Spanje", PloegId = ploegen[8].Id },
            new() { Startnummer = 82, Voornaam = "Alejandro", Achternaam = "Valverde", Land = "Spanje", PloegId = ploegen[8].Id },
            new() { Startnummer = 83, Voornaam = "Ivan", Achternaam = "García Cortina", Land = "Spanje", PloegId = ploegen[8].Id },
            new() { Startnummer = 84, Voornaam = "Nelson", Achternaam = "Oliveira", Land = "Portugal", PloegId = ploegen[8].Id },
            new() { Startnummer = 85, Voornaam = "Pablo", Achternaam = "Castrillo", Land = "Spanje", PloegId = ploegen[8].Id },
            new() { Startnummer = 86, Voornaam = "Einer", Achternaam = "Rubio", Land = "Colombia", PloegId = ploegen[8].Id },
            new() { Startnummer = 87, Voornaam = "Carlos", Achternaam = "Verona", Land = "Spanje", PloegId = ploegen[8].Id },
            new() { Startnummer = 88, Voornaam = "Gregor", Achternaam = "Mühlberger", Land = "Oostenrijk", PloegId = ploegen[8].Id },
            // AG2R (91-98)
            new() { Startnummer = 91, Voornaam = "Ben", Achternaam = "O'Connor", Land = "Australië", PloegId = ploegen[9].Id },
            new() { Startnummer = 92, Voornaam = "Felix", Achternaam = "Gall", Land = "Oostenrijk", PloegId = ploegen[9].Id },
            new() { Startnummer = 93, Voornaam = "Clément", Achternaam = "Champoussin", Land = "Frankrijk", PloegId = ploegen[9].Id },
            new() { Startnummer = 94, Voornaam = "Benoît", Achternaam = "Cosnefroy", Land = "Frankrijk", PloegId = ploegen[9].Id },
            new() { Startnummer = 95, Voornaam = "Larry", Achternaam = "Warbasse", Land = "VS", PloegId = ploegen[9].Id },
            new() { Startnummer = 96, Voornaam = "Aurélien", Achternaam = "Paret-Peintre", Land = "Frankrijk", PloegId = ploegen[9].Id },
            new() { Startnummer = 97, Voornaam = "Dorian", Achternaam = "Godon", Land = "Frankrijk", PloegId = ploegen[9].Id },
            new() { Startnummer = 98, Voornaam = "Oliver", Achternaam = "Naesen", Land = "België", PloegId = ploegen[9].Id },
            // Alpecin-Deceuninck (101-108)
            new() { Startnummer = 101, Voornaam = "Jasper", Achternaam = "Philipsen", Land = "België", PloegId = ploegen[17].Id },
            new() { Startnummer = 102, Voornaam = "Mathieu", Achternaam = "van der Poel", Land = "Nederland", PloegId = ploegen[17].Id },
            new() { Startnummer = 103, Voornaam = "Xandro", Achternaam = "Meurisse", Land = "België", PloegId = ploegen[17].Id },
            new() { Startnummer = 104, Voornaam = "Jonas", Achternaam = "Rickaert", Land = "België", PloegId = ploegen[17].Id },
            new() { Startnummer = 105, Voornaam = "Gianni", Achternaam = "Vermeersch", Land = "België", PloegId = ploegen[17].Id },
            new() { Startnummer = 106, Voornaam = "Edward", Achternaam = "Theuns", Land = "België", PloegId = ploegen[17].Id },
            new() { Startnummer = 107, Voornaam = "Silvan", Achternaam = "Dillier", Land = "Zwitserland", PloegId = ploegen[17].Id },
            new() { Startnummer = 108, Voornaam = "Tim", Achternaam = "Merlier", Land = "België", PloegId = ploegen[17].Id },
            // Groupama-FDJ (111-118)
            new() { Startnummer = 111, Voornaam = "David", Achternaam = "Gaudu", Land = "Frankrijk", PloegId = ploegen[13].Id },
            new() { Startnummer = 112, Voornaam = "Thibaut", Achternaam = "Pinot", Land = "Frankrijk", PloegId = ploegen[13].Id },
            new() { Startnummer = 113, Voornaam = "Stefan", Achternaam = "Küng", Land = "Zwitserland", PloegId = ploegen[13].Id },
            new() { Startnummer = 114, Voornaam = "Rudy", Achternaam = "Molard", Land = "Frankrijk", PloegId = ploegen[13].Id },
            new() { Startnummer = 115, Voornaam = "Jacopo", Achternaam = "Guarnieri", Land = "Italië", PloegId = ploegen[13].Id },
            new() { Startnummer = 116, Voornaam = "Valentin", Achternaam = "Madouas", Land = "Frankrijk", PloegId = ploegen[13].Id },
            new() { Startnummer = 117, Voornaam = "Lars", Achternaam = "van den Berg", Land = "Nederland", PloegId = ploegen[13].Id },
            new() { Startnummer = 118, Voornaam = "Fabian", Achternaam = "Lienhard", Land = "Zwitserland", PloegId = ploegen[13].Id },
            // Intermarché-Circus (121-128)
            new() { Startnummer = 121, Voornaam = "Biniam", Achternaam = "Girmay", Land = "Eritrea", PloegId = ploegen[14].Id },
            new() { Startnummer = 122, Voornaam = "Rein", Achternaam = "Taaramäe", Land = "Estland", PloegId = ploegen[14].Id },
            new() { Startnummer = 123, Voornaam = "Jan", Achternaam = "Hirt", Land = "Tsjechië", PloegId = ploegen[14].Id },
            new() { Startnummer = 124, Voornaam = "Alexander", Achternaam = "Kristoff", Land = "Noorwegen", PloegId = ploegen[14].Id },
            new() { Startnummer = 125, Voornaam = "Mike", Achternaam = "Teunissen", Land = "Nederland", PloegId = ploegen[14].Id },
            new() { Startnummer = 126, Voornaam = "Georg", Achternaam = "Zimmermann", Land = "Duitsland", PloegId = ploegen[14].Id },
            new() { Startnummer = 127, Voornaam = "Kobe", Achternaam = "Goossens", Land = "België", PloegId = ploegen[14].Id },
            new() { Startnummer = 128, Voornaam = "Lorenzo", Achternaam = "Rota", Land = "Italië", PloegId = ploegen[14].Id },
            // Cofidis (131-138)
            new() { Startnummer = 131, Voornaam = "Guillaume", Achternaam = "Martin", Land = "Frankrijk", PloegId = ploegen[11].Id },
            new() { Startnummer = 132, Voornaam = "Jesús", Achternaam = "Herrada", Land = "Spanje", PloegId = ploegen[11].Id },
            new() { Startnummer = 133, Voornaam = "Søren", Achternaam = "Kragh Andersen", Land = "Denemarken", PloegId = ploegen[11].Id },
            new() { Startnummer = 134, Voornaam = "Victor", Achternaam = "Lafay", Land = "Frankrijk", PloegId = ploegen[11].Id },
            new() { Startnummer = 135, Voornaam = "Simone", Achternaam = "Consonni", Land = "Italië", PloegId = ploegen[11].Id },
            new() { Startnummer = 136, Voornaam = "Piet", Achternaam = "Allegaert", Land = "België", PloegId = ploegen[11].Id },
            new() { Startnummer = 137, Voornaam = "Axel", Achternaam = "Zingle", Land = "Frankrijk", PloegId = ploegen[11].Id },
            new() { Startnummer = 138, Voornaam = "Tom", Achternaam = "Bohli", Land = "Zwitserland", PloegId = ploegen[11].Id },
            // DSM-Firmenich (141-148)
            new() { Startnummer = 141, Voornaam = "Romain", Achternaam = "Bardet", Land = "Frankrijk", PloegId = ploegen[12].Id },
            new() { Startnummer = 142, Voornaam = "Nico", Achternaam = "Denz", Land = "Duitsland", PloegId = ploegen[12].Id },
            new() { Startnummer = 143, Voornaam = "Andreas", Achternaam = "Leknessund", Land = "Noorwegen", PloegId = ploegen[12].Id },
            new() { Startnummer = 144, Voornaam = "Casper", Achternaam = "Pedersen", Land = "Denemarken", PloegId = ploegen[12].Id },
            new() { Startnummer = 145, Voornaam = "Nikias", Achternaam = "Arndt", Land = "Duitsland", PloegId = ploegen[12].Id },
            new() { Startnummer = 146, Voornaam = "Chris", Achternaam = "Hamilton", Land = "Australië", PloegId = ploegen[12].Id },
            new() { Startnummer = 147, Voornaam = "Martijn", Achternaam = "Tusveld", Land = "Nederland", PloegId = ploegen[12].Id },
            new() { Startnummer = 148, Voornaam = "Thymen", Achternaam = "Arensman", Land = "Nederland", PloegId = ploegen[12].Id },
            // Lotto Dstny (151-158)
            new() { Startnummer = 151, Voornaam = "Victor", Achternaam = "Campenaerts", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 152, Voornaam = "Maxim", Achternaam = "Van Gils", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 153, Voornaam = "Arnaud", Achternaam = "De Lie", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 154, Voornaam = "Harm", Achternaam = "Vanhoucke", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 155, Voornaam = "Florian", Achternaam = "Vermeersch", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 156, Voornaam = "Sylvain", Achternaam = "Moniquet", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 157, Voornaam = "Thomas", Achternaam = "De Gendt", Land = "België", PloegId = ploegen[16].Id },
            new() { Startnummer = 158, Voornaam = "Lenny", Achternaam = "Marchand", Land = "Frankrijk", PloegId = ploegen[16].Id },
        };

        context.Renners.AddRange(renners);
        context.SaveChanges();

        // Seed 21 etappes
        var etappes = Enumerable.Range(1, 21).Select(i => new Etappe
        {
            EtappeNummer = i,
            EtappeType = i switch
            {
                1 => "Vlak",
                2 => "Vlak",
                3 => "Heuvelachtig",
                4 => "Tijdrit",
                5 => "Vlak",
                6 => "Heuvelachtig",
                7 => "Berg",
                8 => "Berg",
                9 => "Berg",
                10 => "Rustdag",
                11 => "Vlak",
                12 => "Heuvelachtig",
                13 => "Vlak",
                14 => "Berg",
                15 => "Berg",
                16 => "Berg",
                17 => "Rustdag",
                18 => "Vlak",
                19 => "Berg",
                20 => "Tijdrit",
                21 => "Vlak",
                _ => "Onbekend"
            }
        }).ToList();

        context.Etappes.AddRange(etappes);
        context.SaveChanges();
    }
}
