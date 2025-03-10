using System.Text.Json;
using FormGenerator.Models;
using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;

public class FormGenerationService : IFormGenerationService
{
    private readonly ILogger<FormGenerationService> _logger;
    private int _fieldCounter = 0;

    public FormGenerationService(ILogger<FormGenerationService> logger)
    {
        _logger = logger;
    }

    public async Task<FormModel> GenerateFormFromJsonAsync(string jsonContent)
    {
        try
        {
            // Reset counter for each form generation
            _fieldCounter = 0;

            return await Task.Run(() =>
            {
                var formModel = new FormModel();

                // Deserialize the JSON into our form definition
                var formDefinition = JsonSerializer.Deserialize<FormDefinition>(jsonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (formDefinition == null || formDefinition.Fields == null)
                {
                    _logger.LogError("Failed to deserialize JSON form definition");
                    return formModel;
                }

                formModel.Title = formDefinition.Title ?? "Dynamic Form";
                formModel.InputFields = new List<InputField>();
                formModel.DropdownFields = new List<DropdownField>();
                formModel.CheckboxFields = new List<CheckboxField>();

                // Process each field definition
                foreach (var field in formDefinition.Fields)
                {
                    switch (field.Type?.ToLower())
                    {
                        case "text":
                        case "email":
                        case "number":
                            // Create a guaranteed unique ID for every field
                            string uniqueId = GenerateUniqueFieldId(field.Type.ToLower());
                            _logger.LogInformation($"Creating field: {field.Label} with ID: {uniqueId}");

                            var inputField = new InputField
                            {
                                Id = uniqueId,
                                Label = field.Label ?? "Unnamed Field",
                                Value = field.DefaultValue ?? string.Empty,
                                IsRequired = field.Required,
                                InputType = field.Type.ToLower(),
                                Min = field.Min,
                                Max = field.Max
                            };
                            formModel.InputFields.Add(inputField);
                            break;

                        case "dropdown":
                            formModel.DropdownFields.Add(new DropdownField
                            {
                                Id = GenerateUniqueFieldId("dropdown"),
                                Label = field.Label ?? "Unnamed Dropdown",
                                SelectedValue = field.DefaultValue ?? string.Empty,
                                Options = field.Values ?? new List<string>(),
                                IsRequired = field.Required
                            });
                            break;

                        case "checkbox":
                            formModel.CheckboxFields.Add(new CheckboxField
                            {
                                Id = GenerateUniqueFieldId("checkbox"),
                                Label = field.Label ?? "Unnamed Checkbox",
                                IsChecked = field.DefaultValue == "true",
                                IsRequired = field.Required
                            });
                            break;

                        default:
                            _logger.LogWarning($"Unknown field type: {field.Type}");
                            break;
                    }
                }

                // Log all generated field IDs for debugging
                LogGeneratedFieldIds(formModel);

                return formModel;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating form from JSON");
            throw;
        }
    }

    // Generate a unique field ID that won't collide
    private string GenerateUniqueFieldId(string fieldType)
    {
        _fieldCounter++;
        return $"{fieldType}_{DateTime.Now.Ticks}_{_fieldCounter}";
    }

    // Log all field IDs for debugging
    private void LogGeneratedFieldIds(FormModel model)
    {
        _logger.LogInformation("Generated fields with unique IDs:");

        foreach (var field in model.InputFields)
        {
            _logger.LogInformation($"Input field: {field.Label}, ID: {field.Id}, Type: {field.InputType}");
        }

        foreach (var field in model.DropdownFields)
        {
            _logger.LogInformation($"Dropdown field: {field.Label}, ID: {field.Id}");
        }

        foreach (var field in model.CheckboxFields)
        {
            _logger.LogInformation($"Checkbox field: {field.Label}, ID: {field.Id}");
        }
    }
}