using System.Text.Json;
using System.Text;
using Traveler.BlazorServer.Data.Models;
using Polly;

namespace Traveler.BlazorServer.Data.Services
{
    public class JournalService : IJournalService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<JournalService> _logger;

        public JournalService(IHttpClientFactory httpClientFactory, ILogger<JournalService> logger)
        {
            _clientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<List<Journal>> GetJournalAsync(CancellationToken cancellationToken)
        {
            CancellationToken.None.ThrowIfCancellationRequested();

            try
            {
                var url = "";
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("Authorization", "token");

                // create retry policy
                // TODO create separate policy service with selection of policies
                var policy = Policy.Handle<HttpRequestException>().RetryAsync(3);
                HttpResponseMessage response = await policy.ExecuteAsync(async ct => await client.GetAsync(url, ct), cancellationToken);

                response.EnsureSuccessStatusCode();

                var responseStream = await response.Content.ReadAsStreamAsync();
                var journals = await JsonSerializer.DeserializeAsync<List<Journal>>(responseStream);
                return journals != null ? journals : new List<Journal>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
