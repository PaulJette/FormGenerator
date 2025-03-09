namespace FormGenerator.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class FormModel
{
    public string StaticField { get; set; } = string.Empty;
    public List<TextField> InputFields { get; set; } = new();
    public List<DropdownField> DropdownFields { get; set; } = new();
}

public class TextField
{
    public string Id { get; set; }
    public string Label { get; set; }
    public bool IsRequired { get; set; }

    [CustomValidation]
    public string Value { get; set; } = string.Empty;
}

public class DropdownField
{
    public string Id { get; set; }
    public string Label { get; set; }
    public bool IsRequired { get; set; }

    [CustomValidation]
    public string SelectedValue { get; set; } = string.Empty;

    public List<string> Options { get; set; } = new();
}

// Custom attribute to handle conditional validation, Blazor's built in validation components don't automatically bind to dynamic components.
public class CustomValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // For TextField
        if (validationContext.ObjectInstance is TextField textField)
        {
            if (textField.IsRequired && string.IsNullOrWhiteSpace(textField.Value))
            {
                return new ValidationResult("This field is required.");
            }
        }

        // For DropdownField
        else if (validationContext.ObjectInstance is DropdownField dropdownField)
        {
            if (dropdownField.IsRequired && string.IsNullOrWhiteSpace(dropdownField.SelectedValue))
            {
                return new ValidationResult("Please select a value.");
            }
        }

        return ValidationResult.Success;
    }
}