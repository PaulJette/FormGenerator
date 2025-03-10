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
    private readonly ILogger<FormStateService> _logger;

    public FormModel CurrentForm { get; private set; } = new FormModel();
    public FormModel OriginalForm { get; private set; } = new FormModel();
    public Dictionary<string, int?> NumericValues { get; private set; } = new Dictionary<string, int?>();
    public bool IsFormSubmitted { get; set; }
    public EditContext EditContext { get; private set; }

    public FormStateService(
        IFormGenerationService formGenerationService,
        IJsonLoaderService jsonLoaderService,
        ILogger<FormStateService> logger)
    {
        _formGenerationService = formGenerationService;
        _jsonLoaderService = jsonLoaderService;
        _logger = logger;
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

        _logger.LogInformation($"Initializing {CurrentForm.InputFields.Count} numeric fields");

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

                _logger.LogDebug($"Initialized numeric field: {field.Label}, ID: {field.Id}, Value: {NumericValues[field.Id]}");
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
                Id = field.Id,  // Preserve original ID
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
                Id = field.Id,  // Preserve original ID
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
                Id = field.Id,  // Preserve original ID
                Label = field.Label,
                IsRequired = field.IsRequired,
                IsChecked = field.IsChecked
            });
        }

        return clone;
    }

    public void UpdateNumericFieldValue(InputField field, int? value)
    {
        if (field == null)
        {
            _logger.LogWarning("Attempted to update null field");
            return;
        }

        _logger.LogDebug($"Updating numeric field: {field.Label}, ID: {field.Id}, Value: {value}");

        // Update the dictionary value
        NumericValues[field.Id] = value;

        // Update the string value in the model
        field.Value = value?.ToString() ?? string.Empty;

        // Log the state of NumericValues after update
        _logger.LogDebug($"NumericValues now contains {NumericValues.Count} entries");
        foreach (var entry in NumericValues)
        {
            _logger.LogDebug($"  ID: {entry.Key}, Value: {entry.Value}");
        }

        // Notify validation system of change
        var fieldIdentifier = new FieldIdentifier(field, nameof(InputField.Value));
        EditContext.NotifyFieldChanged(fieldIdentifier);
    }
}