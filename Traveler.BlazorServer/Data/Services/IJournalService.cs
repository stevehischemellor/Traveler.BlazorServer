using Traveler.BlazorServer.Data.Models;

namespace Traveler.BlazorServer.Data.Services
{
    public interface IJournalService
    {
        Task<List<Journal>> GetJournalAsync(CancellationToken cancellationToken);
    }
}