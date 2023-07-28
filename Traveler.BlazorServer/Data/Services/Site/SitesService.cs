﻿using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using System.Net;
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
                var pollyContext = new Context("Retry 500");
                var policy = Policy
                    .Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.InternalServerError)
                    .WaitAndRetryAsync(
                        5,
                        _ => TimeSpan.FromMilliseconds(500),
                        (result, timespan, retryNo, context) =>
                        {
                            _logger.LogInformation($"{context.OperationKey}: Retry number {retryNo} within " +
                                $"{timespan.TotalMilliseconds}ms.");
                        }
                    );

                var response = await policy.ExecuteAsync(async ctx =>
                {
                    var url = string.Format("{0}{1}", _configuration.ApiBaseUrl, "parks");
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("x-api-key", _configuration.ApiKey);
                    var response = await _httpClient.SendAsync(request, cancellationToken);
                    response.EnsureSuccessStatusCode();
                    return response;
                }, pollyContext);
                
                var stm = await response.Content.ReadAsStreamAsync();
                var npsResponse = await JsonSerializer.DeserializeAsync<NpsResponse<List<Site>>>(stm);
                return npsResponse.Data == null ? new List<Site>() : npsResponse.Data;

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
                var npsResponse = await JsonSerializer.DeserializeAsync<NpsResponse<Site>>(stm);
                return npsResponse.Data;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
