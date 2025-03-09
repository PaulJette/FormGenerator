using Microsoft.AspNetCore.Components.Forms;
using FormGenerator.Models;

namespace FormGenerator.Services.Interfaces;

/// <summary>
/// Provides validation for dynamic forms
/// </summary>
public interface IFormValidationService
{
    /// <summary>
    /// Validates the entire form
    /// </summary>
    bool ValidateForm(FormModel formModel, EditContext editContext);

    /// <summary>
    /// Checks if a dropdown has validation errors
    /// </summary>
    bool HasDropdownError(DropdownField dropdown, bool formSubmitted);

    /// <summary>
    /// Returns an email validation function for MudBlazor
    /// </summary>
    Func<string, string> GetEmailValidator(InputField field);

    /// <summary>
    /// Validates an email value
    /// </summary>
    string ValidateEmail(InputField field, string value);

    /// <summary>
    /// Checks if a string is a valid email
    /// </summary>
    bool IsValidEmail(string email);
}