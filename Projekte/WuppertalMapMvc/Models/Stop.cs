using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WuppertalMapMvc.Models
{
    public class Stop
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // X/Y Position für symbolische Netzkarte (NextSkills-Stil)
        public int X { get; set; }
        public int Y { get; set; }

        // Klassische Koordinaten für GPS-Karte (Leaflet)
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? OpeningHours { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Linien als CSV (für Speicherung in der DB)
        public string LinesRaw { get; set; } = string.Empty;

        [NotMapped]
        public string[] Lines
        {
            get => LinesRaw?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            set => LinesRaw = string.Join(",", value ?? Array.Empty<string>());
        }
    }
}
