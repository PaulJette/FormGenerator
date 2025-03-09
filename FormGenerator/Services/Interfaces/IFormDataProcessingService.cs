using FormGenerator.Models;

namespace FormGenerator.Services.Interfaces;

/// <summary>
/// Processes form data into different formats
/// </summary>
public interface IFormDataProcessingService
{
    /// <summary>
    /// Converts form data to formatted JSON
    /// </summary>
    string ProcessFormData(FormModel formModel, Dictionary<string, int?> numericValues);
}