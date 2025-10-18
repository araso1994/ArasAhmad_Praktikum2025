namespace WuppertalMapMvc.Models
{
    public class StopNetworkResult
    {
        public List<Stop> Stops { get; set; } = new();
        public List<StopConnection> Connections { get; set; } = new();
    }
}
