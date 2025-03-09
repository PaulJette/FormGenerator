using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;

public class JsonLoaderService : IJsonLoaderService
{
    private readonly ILogger<JsonLoaderService> _logger;
    private readonly HttpClient _httpClient;

    public JsonLoaderService(ILogger<JsonLoaderService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<string> LoadJsonFromWwwrootAsync(string fileName)
    {
        try
        {
            // In WebAssembly, we access files via HTTP instead of file system
            var response = await _httpClient.GetAsync(fileName);

            if (!response.IsSuccessStatusCode)
            {
                throw new FileNotFoundException($"Configuration file not found: {fileName}");
            }

            // Read the content as string
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error loading JSON from file: {fileName}");
            throw;
        }
    }
}