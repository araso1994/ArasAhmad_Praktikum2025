using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WuppertalMapMvc.Models
{
    public class InsertBetweenViewModel
    {
        [Display(Name = "Start-Haltestelle")]
        public int StartStopId { get; set; }

        [Display(Name = "End-Haltestelle")]
        public int EndStopId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Lines { get; set; } = string.Empty; // z.B. "603,611"

        [Required]
        [Display(Name = "Breitengrad")]
        public double Latitude { get; set; }

        [Required]
        [Display(Name = "Längengrad")]
        public double Longitude { get; set; }

        // Liste für Dropdown-Auswahl
        public List<SelectListItem> StopList { get; set; } = new();
    }
}
