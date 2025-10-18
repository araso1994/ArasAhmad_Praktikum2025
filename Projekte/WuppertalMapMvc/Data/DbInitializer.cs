using WuppertalMapMvc.Models;

namespace WuppertalMapMvc.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Stops.Any())
            {
                var stops = new List<Stop>
        {
            new Stop
            {
                Name = "Wuppertal Hauptbahnhof",
                Lines = new[] { "603", "611", "628" },
                Latitude = 51.2562,
                Longitude = 7.1508,
                X = 100,
                Y = 200,
                Description = "Die verkehrsreichste Haltestelle im Zentrum Elberfelds.",
                ImagePath = "wuppertal-hauptbahnhof.jpg",
                OpeningHours = "24/7"
            },
            new Stop
            {
                Name = "Zoo",
                Lines = new[] { "615", "628" },
                Latitude = 51.243,
                Longitude = 7.123,
                X = 200,
                Y = 200,
                Description = "Der Zoologische Garten – eine der Top-Sehenswürdigkeiten Wuppertals.",
                ImagePath = "zoo.jpg",
                OpeningHours = "09:00 - 18:00"
            },
            new Stop
            {
                Name = "Ohligsmühle",
                Lines = new[] { "601", "640" },
                Latitude = 51.2567,
                Longitude = 7.148,
                X = 150,
                Y = 250,
                Description = "Ehemalige Wassermühle an der Wupper – heute ein Verkehrsknoten.",
                ImagePath = "Ohlingsmühle.jpeg",
                OpeningHours = "immer"
            }
        };

                context.Stops.AddRange(stops);
                context.SaveChanges();
            }

            if (!context.StopConnections.Any())
            {
                var hbf = context.Stops.FirstOrDefault(s => s.Name == "Wuppertal Hauptbahnhof");
                var zoo = context.Stops.FirstOrDefault(s => s.Name == "Zoo");
                var ohligs = context.Stops.FirstOrDefault(s => s.Name == "Ohligsmühle");

                if (hbf != null && zoo != null && ohligs != null)
                {
                    var connections = new List<StopConnection>
            {
                new StopConnection { FromStopId = hbf.Id, ToStopId = zoo.Id, Line = "628" },
                new StopConnection { FromStopId = hbf.Id, ToStopId = ohligs.Id, Line = "603" }
            };

                    context.StopConnections.AddRange(connections);
                    context.SaveChanges();
                }
            }
        }


    }
}