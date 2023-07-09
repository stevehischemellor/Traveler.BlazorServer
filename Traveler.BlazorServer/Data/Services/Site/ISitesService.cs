using Traveler.BlazorServer.Data.Models;

namespace Traveler.BlazorServer.Data.Services
{
    public interface ISitesService
    {
        Task<Site> GetSiteAsync(string parkCode, CancellationToken cancellationToken);
        Task<List<Site>> GetSitesAsync(CancellationToken cancellationToken);
    }
}