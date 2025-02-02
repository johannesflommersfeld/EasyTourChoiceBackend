using EasyTourChoice.API.Controllers.Interfaces;

namespace EasyTourChoice.API.Application.DataAggregation;

public class HttpService(ILogger<HttpService> logger, IHttpClientFactory httpClientFactory) : IHttpService
{
    private readonly ILogger _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<Stream> PerformGetRequestAsync(string url, string? userAgent = null)
    {
        using HttpClient client = _httpClientFactory.CreateClient();
        if (userAgent is not null)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", userAgent);
        }
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var httpResponse = await client.SendAsync(request);
            return await httpResponse.Content.ReadAsStreamAsync();
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
