using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LernPfad.Models
{

    public class Lernschritt
    {
        public int Id { get; set; }

        [Required]
        public string Titel { get; set; }

        public string Inhalt { get; set; }

        public int Reihenfolge { get; set; }

        // Fremdschlüssel
        public int LernpfadId { get; set; }

        // Navigation
        [ForeignKey("LernpfadId")]
        public Lernpfad Lernpfad { get; set; }
    }


}
