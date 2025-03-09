namespace FormGenerator.Services.Implementations;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using FormGenerator.Models;

public class FormService
{
    private readonly ILogger<FormService> _logger;
    private readonly Random _random = new Random();

    public FormService(ILogger<FormService> logger)
    {
        _logger = logger;
    }

    public void GenerateForm(FormModel model)
    {
        GenerateTextFields(model);
        GenerateDropdowns(model);
    }

    private void GenerateTextFields(FormModel model)
    {
        int numberOfFields = _random.Next(1, 5); // 1-4 input fields
        model.InputFields = new List<TextField>();

        for (int i = 1; i <= numberOfFields; i++)
        {
            model.InputFields.Add(new TextField
            {
                Id = $"Field{i}",
                Label = $"Field {i}",
                Value = string.Empty
            });
        }
    }

    private void GenerateDropdowns(FormModel model)
    {
        int numberOfDropdowns = _random.Next(1, 4); // 1-3 dropdowns
        model.DropdownFields = new List<DropdownField>();

        for (int i = 1; i <= numberOfDropdowns; i++)
        {
            model.DropdownFields.Add(new DropdownField
            {
                Id = $"Dropdown{i}",
                Label = $"Dropdown {i}",
                SelectedValue = string.Empty,
                Options = new List<string> { "Option 1", "Option 2", "Option 3" }
            });
        }
    }

    public bool ValidateForm(FormModel model)
    {
        bool isValid = !string.IsNullOrWhiteSpace(model.StaticField);

        // Validate input fields
        foreach (var field in model.InputFields)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                isValid = false;
                break;
            }
        }

        // Validate dropdown fields
        if (isValid)
        {
            foreach (var dropdown in model.DropdownFields)
            {
                if (string.IsNullOrWhiteSpace(dropdown.SelectedValue))
                {
                    isValid = false;
                    break;
                }
            }
        }

        return isValid;
    }

    public void ProcessForm(FormModel model)
    {
        _logger.LogInformation("Form submitted successfully!");
        _logger.LogInformation($"Static Field: {model.StaticField}");

        foreach (var field in model.InputFields)
        {
            _logger.LogInformation($"Field: {field.Label}, Value: {field.Value}");
        }

        foreach (var dropdown in model.DropdownFields)
        {
            _logger.LogInformation($"Dropdown: {dropdown.Label}, Selected Value: {dropdown.SelectedValue}");
        }
    }
}