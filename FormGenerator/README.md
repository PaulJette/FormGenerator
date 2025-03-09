# Dynamic Form Generator

A flexible Blazor Server application that generates form elements dynamically from JSON configuration files. The form supports multiple input types, validation, and exports data as JSON.

![Version](https://img.shields.io/badge/version-1.0.0-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

## 📋 Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Form Configuration](#form-configuration)
- [Technical Design](#technical-design)
- [Implementation Notes](#implementation-notes)
- [License](#license)

## ✨ Features

- **Dynamic Form Generation**: Create forms from JSON configuration files
- **Multiple Input Types**: Support for text, email, number, dropdown, and checkbox fields
- **Validation**: Built-in validation based on field requirements
- **JSON Output**: Export form data as formatted JSON
- **Error Handling**: Robust error handling with user-friendly error displays

## 🚀 Installation

1. Clone the repository
   ```
   git clone https://github.com/pauljette/dynamic-form-generator.git
   ```

2. Navigate to the project directory
   ```
   cd dynamic-form-generator
   ```

3. Build the project
   ```
   dotnet build
   ```

4. Run the application
   ```
   dotnet run
   ```

## 🔧 Usage

1. Create or modify the `wwwroot/formConfig.json` file with your form definition
2. Run the application
3. Fill out the form
4. Click Submit to see the JSON output

## 📝 Form Configuration

The form is configured using a JSON file with the following structure:

```json
{
  "title": "Sample Form",
  "fields": [
    {
      "type": "text",
      "label": "Name",
      "required": true
    },
    {
      "type": "email",
      "label": "Email",
      "required": true
    },
    {
      "type": "number",
      "label": "Age",
      "min": 18,
      "max": 100
    },
    {
      "type": "dropdown",
      "label": "Industry",
      "values": [ "Tech", "Production", "Health" ],
      "required": true
    },
    {
      "type": "checkbox",
      "label": "Subscribe to Newsletter",
      "required": false
    }
  ]
}
```

### Supported Field Types

| Type | Description | Additional Properties |
|------|-------------|------------------------|
| text | Text input field | - |
| email | Email input with validation | - |
| number | Numeric input | min, max |
| dropdown | Select dropdown | values (array) |
| checkbox | Boolean checkbox | - |

## 🏗️ Technical Design

### Project Structure

```
DynamicFormGenerator/
├── Pages/
│   └── DynamicForm.razor       # Main form component
├── Components/
│   └── ErrorHandling.razor     # Error display component
├── Models/
│   └── FormModels.cs           # Form model classes
├── Services/
│   ├── Interfaces/
│   │   ├── IFormGenerationService.cs
│   │   └── IJsonLoaderService.cs
│   └── Implementations/
│       ├── FormGenerationService.cs
│       └── JsonLoaderService.cs
└── wwwroot/
    └── formConfig.json         # JSON configuration file
```

### Key Components

1. **Services**
   - `IFormGenerationService`: Interface for form generation
   - `FormGenerationService`: Generates form models from JSON
   - `IJsonLoaderService`: Interface for loading JSON
   - `JsonLoaderService`: Loads JSON from file system

2. **Models**
   - `FormModel`: Main form container
   - `InputField`: Base class for text, email, and number inputs
   - `DropdownField`: Select dropdown inputs
   - `CheckboxField`: Boolean checkbox inputs

3. **Components**
   - `DynamicForm`: Main form component
   - `ErrorHandling`: Error display component

## 📒 Implementation Notes

### Form Validation

Initially, validation for dynamic fields wasn't working correctly. I solved this by:

1. Using direct HTML inputs with manual validation for number fields:
   ```csharp
   <input type="number" value="@field.Value" 
          @onchange="@(e => field.Value = e.Value?.ToString() ?? string.Empty)" />
   ```

2. Using the built-in Blazor components for other fields:
   ```csharp
   <InputText @bind-Value="field.Value" />
   ```

3. Adding inline validation messages:
   ```csharp
   @if (field.IsRequired && string.IsNullOrEmpty(field.Value))
   {
       <span class="field-validation-error">This field is required.</span>
   }
   ```

This gives me more control over the validation process, which is crucial for dynamically generated forms.

### Service-Based Architecture

I chose a service-based architecture to:

1. Separate concerns (UI, business logic, data access)
2. Make the code more testable
3. Improve maintainability

The form generation and JSON loading are handled by separate services, each with a focused responsibility.

### Interface-Based Design

I introduced interfaces to:

1. Facilitate dependency injection
2. Allow for easier testing with mock implementations
3. Decouple components from concrete implementations

This follows the Dependency Inversion Principle from SOLID design principles.

### JSON Generation

The form data is collected and serialized to JSON with:

```csharp
var formData = new Dictionary<string, object>();
// Populate dictionary with form values
formDataJson = JsonSerializer.Serialize(formData, options);
```

I ensured numbers are properly parsed to appear as numbers in the JSON output, not strings.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.