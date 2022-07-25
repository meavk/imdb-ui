using System.Text.Json;

namespace Imdb.Blazor.Data;

public class ImdbService
{
    private readonly HttpClient? imdbServiceClient;

    public ImdbService(IHttpContextAccessor httpContextAccessor)
    {
        this.imdbServiceClient = new HttpClient(); 
        var urlString = Environment.GetEnvironmentVariable("IMDB_SERVICE_URL")
            ?? $"{httpContextAccessor?.HttpContext?.Request.Scheme}://{httpContextAccessor?.HttpContext?.Request.Host.Value}";
        this.imdbServiceClient.BaseAddress = new Uri(urlString);
    }

    public async Task<Movie[]> GetMovieAsync()
    {
        var moviesResult = await this.imdbServiceClient!.GetAsync("/api/movies");
        moviesResult.EnsureSuccessStatusCode();
        var moviesJson = await moviesResult.Content.ReadAsStringAsync();

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(moviesJson, serializeOptions);
        return movies!.Take(10).ToArray();
    }
}
