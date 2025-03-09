using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;

public class JsonLoaderService : IJsonLoaderService
{
    private readonly ILogger<JsonLoaderService> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public JsonLoaderService(ILogger<JsonLoaderService> logger, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<string> LoadJsonFromWwwrootAsync(string fileName)
    {
        try
        {
            // Construct the full path to the file in wwwroot
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Configuration file not found: {fileName}", filePath);
            }

            // Read the file content
            return await File.ReadAllTextAsync(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error loading JSON from wwwroot file: {fileName}");
            throw;
        }
    }
}

