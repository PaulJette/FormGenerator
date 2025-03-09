using FormGenerator.Models;

namespace FormGenerator.Services.Interfaces;

/// <summary>
/// Interface for service that generates form models from JSON configuration
/// </summary>
public interface IFormGenerationService
{
    /// <summary>
    /// Generates a form model from a JSON string
    /// </summary>
    /// <param name="jsonContent">JSON content containing form definition</param>
    /// <returns>A FormModel representing the parsed form</returns>
    Task<FormModel> GenerateFormFromJsonAsync(string jsonContent);
}


