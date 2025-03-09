using Microsoft.AspNetCore.Components.Forms;
using FormGenerator.Models;
using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;

/// <summary>
/// Manages the state of dynamic forms
/// </summary>
public class FormStateService : IFormStateService
{
    private readonly IFormGenerationService _formGenerationService;
    private readonly IJsonLoaderService _jsonLoaderService;

    public FormModel CurrentForm { get; private set; } = new FormModel();
    public FormModel OriginalForm { get; private set; } = new FormModel();
    public Dictionary<string, int?> NumericValues { get; private set; } = new Dictionary<string, int?>();
    public bool IsFormSubmitted { get; set; }
    public EditContext EditContext { get; private set; }

    public FormStateService(
        IFormGenerationService formGenerationService,
        IJsonLoaderService jsonLoaderService)
    {
        _formGenerationService = formGenerationService;
        _jsonLoaderService = jsonLoaderService;
        EditContext = new EditContext(CurrentForm);
    }

    public async Task InitializeFormAsync(string jsonFilePath)
    {
        // Load JSON from the wwwroot folder
        string jsonContent = await _jsonLoaderService.LoadJsonFromWwwrootAsync(jsonFilePath);

        // Generate form model from the loaded JSON
        CurrentForm = await _formGenerationService.GenerateFormFromJsonAsync(jsonContent);

        // Create edit context for form validation
        EditContext = new EditContext(CurrentForm);

        // Create a deep copy for reset functionality
        OriginalForm = CloneFormModel(CurrentForm);

        // Initialize numeric fields dictionary
        InitializeNumericFields();
    }

    /// <summary>
    /// Populates NumericValues dictionary from string values in InputFields
    /// </summary>
    private void InitializeNumericFields()
    {
        NumericValues.Clear();
        foreach (var field in CurrentForm.InputFields)
        {
            if (field.InputType == "number")
            {
                if (int.TryParse(field.Value, out int val))
                {
                    NumericValues[field.Id] = val;
                }
                else
                {
                    NumericValues[field.Id] = null;
                }
            }
        }
    }

    public void ResetForm()
    {
        // Reset form submitted state
        IsFormSubmitted = false;

        // Reset form to original values
        CurrentForm = CloneFormModel(OriginalForm);

        // Create a new EditContext
        EditContext = new EditContext(CurrentForm);

        // Reset numeric values dictionary
        InitializeNumericFields();
    }

    public FormModel CloneFormModel(FormModel source)
    {
        var clone = new FormModel
        {
            Title = source.Title,
            InputFields = new List<InputField>(),
            DropdownFields = new List<DropdownField>(),
            CheckboxFields = new List<CheckboxField>()
        };

        // Clone input fields
        foreach (var field in source.InputFields)
        {
            clone.InputFields.Add(new InputField
            {
                Id = field.Id,
                Label = field.Label,
                InputType = field.InputType,
                IsRequired = field.IsRequired,
                Min = field.Min,
                Max = field.Max,
                Value = field.Value
            });
        }

        // Clone dropdown fields
        foreach (var field in source.DropdownFields)
        {
            clone.DropdownFields.Add(new DropdownField
            {
                Id = field.Id,
                Label = field.Label,
                IsRequired = field.IsRequired,
                SelectedValue = field.SelectedValue,
                Options = new List<string>(field.Options)
            });
        }

        // Clone checkbox fields
        foreach (var field in source.CheckboxFields)
        {
            clone.CheckboxFields.Add(new CheckboxField
            {
                Id = field.Id,
                Label = field.Label,
                IsRequired = field.IsRequired,
                IsChecked = field.IsChecked
            });
        }

        return clone;
    }

    public void UpdateNumericFieldValue(InputField field, int? value)
    {
        // Update the dictionary value
        NumericValues[field.Id] = value;

        // Update the string value in the model
        field.Value = value?.ToString() ?? string.Empty;

        // Notify validation system of change
        var fieldIdentifier = FieldIdentifier.Create(() => field.Value);
        EditContext.NotifyFieldChanged(fieldIdentifier);
    }
}