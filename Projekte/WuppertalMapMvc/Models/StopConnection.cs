using System.ComponentModel.DataAnnotations;

namespace WuppertalMapMvc.Models
{
    public class StopConnection
    {
        [Key]
        public int Id { get; set; }

        public int FromStopId { get; set; }

        public int ToStopId { get; set; }

        public string Line { get; set; }
    }
}
