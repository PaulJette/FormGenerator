using System.Text.Json;
using FormGenerator.Models;
using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;


public class FormGenerationService : IFormGenerationService
{
    private readonly ILogger<FormGenerationService> _logger;

    public FormGenerationService(ILogger<FormGenerationService> logger)
    {
        _logger = logger;
    }

    public async Task<FormModel> GenerateFormFromJsonAsync(string jsonContent)
    {
        try
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
                        var inputField = new InputField
                        {
                            Id = field.Id ?? $"field_{Guid.NewGuid()}",
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
                            Id = field.Id ?? $"dropdown_{Guid.NewGuid()}",
                            Label = field.Label ?? "Unnamed Dropdown",
                            SelectedValue = field.DefaultValue ?? string.Empty,
                            Options = field.Values ?? new List<string>(),
                            IsRequired = field.Required
                        });
                        break;

                    case "checkbox":
                        formModel.CheckboxFields.Add(new CheckboxField
                        {
                            Id = field.Id ?? $"checkbox_{Guid.NewGuid()}",
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

            return formModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating form from JSON");
            throw;
        }
    }
}

