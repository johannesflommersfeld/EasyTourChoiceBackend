namespace EasyTourChoice.API.Controllers.Interfaces;

public interface IHttpService
{
    Task<Stream> PerformGetRequestAsync(string url, string? userAgent = null);
}