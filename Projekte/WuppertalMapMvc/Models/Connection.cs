using System.ComponentModel.DataAnnotations;

namespace WuppertalMapMvc.Models
{
    public class Connection
    {
        [Key]
        public int Id { get; set; }

        public int FromStopId { get; set; }
        public int ToStopId { get; set; }

        [Required]
        public string Line { get; set; } = string.Empty;
    }
}
