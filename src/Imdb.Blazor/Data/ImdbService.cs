using System.Text.Json;

namespace Imdb.Blazor.Data;

public class ImdbService
{
    private HttpClient? imdbServiceClient;

    public ImdbService()
    {
        this.imdbServiceClient = new HttpClient();

        // TODO: Get this dynamically.
        this.imdbServiceClient.BaseAddress = new Uri("http://localhost:8080");
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
