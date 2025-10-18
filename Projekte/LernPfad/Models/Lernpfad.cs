using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LernPfad.Models
{
    public class Lernpfad
    {
        public int Id { get; set; }

        [Required]
        public string? Titel { get; set; }

        public string Beschreibung { get; set; }

        public string AutorId { get; set; }

        public string? BildPfad { get; set; }

        // Navigation
        public List<Lernschritt> Schritte { get; set; } = new(); // ✅ Initialisiert
    }
}
