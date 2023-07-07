namespace Traveler.BlazorServer.Data.Models
{
    public class Journal
    {
        public DateTime Date { get; set; }

        public List<Photo>? Photos { get; set; }

        public List<Entry>? Entries { get; set; }
    }
}