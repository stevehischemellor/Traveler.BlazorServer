namespace Traveler.BlazorServer.Data
{
    public class Journal
    {
        public DateTime Date { get; set; }

        public List<Photo>? Photos { get; set; }

        public List<Entry>? Entries { get; set; }

        public Task<Journal[]> GetJournalAsync()
        {
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new Journal
            {


            }).ToArray());
        }
    }
}