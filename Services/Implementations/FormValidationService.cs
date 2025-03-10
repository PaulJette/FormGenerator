using Microsoft.AspNetCore.Components.Forms;
using FormGenerator.Models;
using FormGenerator.Services.Interfaces;

namespace FormGenerator.Services.Implementations;

/// <summary>
/// Validates dynamic forms
/// </summary>
public class FormValidationService : IFormValidationService
{
    public bool ValidateForm(FormModel formModel, EditContext editContext)
    {
        // Standard validation
        bool standardValidation = editContext.Validate();

        // Custom validation
        bool customValidation = true;

        // Check required dropdowns
        foreach (var dropdown in formModel.DropdownFields)
        {
            if (dropdown.IsRequired && string.IsNullOrWhiteSpace(dropdown.SelectedValue))
            {
                customValidation = false;
                break;
            }
        }

        // Check required checkboxes
        foreach (var checkbox in formModel.CheckboxFields)
        {
            if (checkbox.IsRequired && !checkbox.IsChecked)
            {
                customValidation = false;
                break;
            }
        }

        return standardValidation && customValidation;
    }

    public bool HasDropdownError(DropdownField dropdown, bool formSubmitted)
    {
        // Only show errors after form submission
        if (!formSubmitted)
        {
            return false;
        }

        // Check if dropdown is required and empty
        return dropdown.IsRequired && string.IsNullOrWhiteSpace(dropdown.SelectedValue);
    }

    public Func<string, string> GetEmailValidator(InputField field)
    {
        // Return a validation function for MudBlazor's Validation property
        return value => ValidateEmail(field, value);
    }

    public string ValidateEmail(InputField field, string value)
    {
        // Check required
        if (field.IsRequired && string.IsNullOrWhiteSpace(value))
        {
            return $"{field.Label} is required.";
        }

        // Check email format
        if (!string.IsNullOrWhiteSpace(value) && !IsValidEmail(value))
        {
            return "Please enter a valid email address.";
        }

        // Valid - return empty string instead of null
        return string.Empty;
    }

    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}