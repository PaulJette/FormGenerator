using System.ComponentModel.DataAnnotations;

namespace FormGenerator.Models;

public class FormModel
{
    public string Title { get; set; } = string.Empty;
    public List<InputField> InputFields { get; set; } = new();
    public List<DropdownField> DropdownFields { get; set; } = new();
    public List<CheckboxField> CheckboxFields { get; set; } = new();
}

public class InputField
{
    public string Id { get; set; }
    public string Label { get; set; }
    public bool IsRequired { get; set; }
    public string InputType { get; set; } = "text"; // text, email, number, etc.
    public int? Min { get; set; }
    public int? Max { get; set; }

    [CustomInputValidation]
    public string Value { get; set; } = string.Empty;
}

public class DropdownField
{
    public string Id { get; set; }
    public string Label { get; set; }
    public bool IsRequired { get; set; }

    [CustomDropdownValidation]
    public string SelectedValue { get; set; } = string.Empty;

    public List<string> Options { get; set; } = new();
}

public class CheckboxField
{
    public string Id { get; set; }
    public string Label { get; set; }
    public bool IsRequired { get; set; }

    [CustomCheckboxValidation]
    public bool IsChecked { get; set; }
}

// Custom validation attributes
public class CustomInputValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var field = (InputField)validationContext.ObjectInstance;

        if (field.IsRequired && string.IsNullOrWhiteSpace(field.Value))
        {
            return new ValidationResult($"{field.Label} is required.");
        }

        if (field.InputType == "number" && !string.IsNullOrWhiteSpace(field.Value))
        {
            if (int.TryParse(field.Value, out int numValue))
            {
                if (field.Min.HasValue && numValue < field.Min.Value)
                {
                    return new ValidationResult($"{field.Label} must be at least {field.Min.Value}.");
                }

                if (field.Max.HasValue && numValue > field.Max.Value)
                {
                    return new ValidationResult($"{field.Label} must not exceed {field.Max.Value}.");
                }
            }
            else
            {
                return new ValidationResult($"{field.Label} must be a valid number.");
            }
        }

        if (field.InputType == "email" && !string.IsNullOrWhiteSpace(field.Value))
        {
            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(field.Value))
            {
                return new ValidationResult($"{field.Label} must be a valid email address.");
            }
        }

        return ValidationResult.Success;
    }
}

public class CustomDropdownValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var field = (DropdownField)validationContext.ObjectInstance;

        if (field.IsRequired && string.IsNullOrWhiteSpace(field.SelectedValue))
        {
            return new ValidationResult($"Please select a {field.Label.ToLower()}.");
        }

        return ValidationResult.Success;
    }
}

public class CustomCheckboxValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var field = (CheckboxField)validationContext.ObjectInstance;

        if (field.IsRequired && !field.IsChecked)
        {
            return new ValidationResult($"{field.Label} must be checked.");
        }

        return ValidationResult.Success;
    }
}