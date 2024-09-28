namespace EasyTourChoice.API.Services;

public class HttpService(ILogger<HttpService> logger, IHttpClientFactory httpClientFactory) : IHttpService
{
    private readonly ILogger _logger = logger;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<Stream> PerformGetRequestAsync(string url, string? userAgent = null)
    {
        using HttpClient client = _httpClientFactory.CreateClient();
        if (userAgent is not null)
        {
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }
        Stream? response = null;
        try
        {
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            var httpResponse = await client.SendAsync(message);
            response = await httpResponse.Content.ReadAsStreamAsync();
            return response;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e.Message);
            response?.Dispose();
            throw;
        }
    }
}
