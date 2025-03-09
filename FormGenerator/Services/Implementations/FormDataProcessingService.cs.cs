using System.Text.Json;
using FormGenerator.Models;
using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;

/// <summary>
/// Processes form data into different formats
/// </summary>
public class FormDataProcessingService : IFormDataProcessingService
{
    public string ProcessFormData(FormModel formModel, Dictionary<string, int?> numericValues)
    {
        // Create dictionary for JSON conversion
        var formData = new Dictionary<string, object>();

        // Process input fields
        foreach (var field in formModel.InputFields)
        {
            // Convert number fields to actual numbers in JSON
            if (field.InputType == "number" && !string.IsNullOrEmpty(field.Value) &&
                int.TryParse(field.Value, out int numValue))
            {
                formData[field.Label] = numValue;
            }
            else
            {
                formData[field.Label] = field.Value;
            }
        }

        // Process dropdown fields
        foreach (var dropdown in formModel.DropdownFields)
        {
            formData[dropdown.Label] = dropdown.SelectedValue ?? string.Empty;
        }

        // Process checkbox fields
        foreach (var checkbox in formModel.CheckboxFields)
        {
            formData[checkbox.Label] = checkbox.IsChecked;
        }

        // Format as nice JSON
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        return JsonSerializer.Serialize(formData, options);
    }
}