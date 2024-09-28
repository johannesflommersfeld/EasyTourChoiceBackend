namespace EasyTourChoice.API.Services;

public interface IHttpService
{
    Task<Stream> PerformGetRequestAsync(string url, string? userAgent = null);
}