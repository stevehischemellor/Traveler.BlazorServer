using Microsoft.Extensions.Options;
using Polly;
using System.Text.Json;
using Traveler.BlazorServer.Data.Models;

namespace Traveler.BlazorServer.Data.Services
{
    public class SitesService : ISitesService
    {
        private readonly HttpClient _httpClient;
        private readonly SitesServiceConfiguration _configuration;
        private readonly ILogger<SitesService> _logger;

        public SitesService(IHttpClientFactory httpClientFactory, IOptions<SitesServiceConfiguration> siteConfiguration, ILogger<SitesService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = siteConfiguration.Value;
            _logger = logger;
        }

        public async Task<List<Site>> GetSitesAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) { }
            try
            {
                var url = string.Format(_configuration.ApiBaseUrl, "");
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-api-key", _configuration.ApiKey);
                var response = await _httpClient.SendAsync(request, cancellationToken);
                
                response.EnsureSuccessStatusCode();
                var stm = await response.Content.ReadAsStreamAsync();
                var sites = await JsonSerializer.DeserializeAsync<List<Site>>(stm);
                return sites == null ? new List<Site>() : sites;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Site> GetSiteAsync(string parkCode, CancellationToken cancellationToken)
        {

            try
            {
                var url = string.Format(_configuration.ApiBaseUrl, "");
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-api-key", _configuration.ApiKey);
                var response = await _httpClient.SendAsync(request, cancellationToken);                 
                response.EnsureSuccessStatusCode();
                var stm = await response.Content.ReadAsStreamAsync();
                var site = await JsonSerializer.DeserializeAsync<Site>(stm);
                return site;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
