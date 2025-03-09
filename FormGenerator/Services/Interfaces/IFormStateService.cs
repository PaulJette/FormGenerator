using Microsoft.AspNetCore.Components.Forms;
using FormGenerator.Models;

namespace FormGenerator.Services.Interfaces;

/// <summary>
/// Manages the state of dynamic forms
/// </summary>
public interface IFormStateService
{
    /// <summary>
    /// Current form model being edited
    /// </summary>
    FormModel CurrentForm { get; }

    /// <summary>
    /// Original unmodified form for resetting
    /// </summary>
    FormModel OriginalForm { get; }

    /// <summary>
    /// Dictionary of numeric field values (for MudNumericField)
    /// </summary>
    Dictionary<string, int?> NumericValues { get; }

    /// <summary>
    /// Whether the form has been submitted (for validation display)
    /// </summary>
    bool IsFormSubmitted { get; set; }

    /// <summary>
    /// Form's EditContext for validation
    /// </summary>
    EditContext EditContext { get; }

    /// <summary>
    /// Loads the form from a JSON file
    /// </summary>
    Task InitializeFormAsync(string jsonFilePath);

    /// <summary>
    /// Resets the form to its original state
    /// </summary>
    void ResetForm();

    /// <summary>
    /// Creates a deep copy of a form model
    /// </summary>
    FormModel CloneFormModel(FormModel source);

    /// <summary>
    /// Updates numeric field value and string representation
    /// </summary>
    void UpdateNumericFieldValue(InputField field, int? value);
}