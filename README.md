# Dynamic Form Generator

A Blazor WebAssembly application that dynamically generates forms based on JSON configuration files. Built with MudBlazor for a clean, responsive Material Design UI.

## Features

- **Dynamic form generation** from JSON configuration
- **Multiple field types** supported (text, email, number, dropdown, checkbox)
- **Form validation** with custom rules and error messages
- **Responsive design** using MudBlazor components
- **Form state management** including reset functionality
- **JSON output** of form data

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- A modern web browser

### Running the Application

1. Clone the repository to your local machine
   ```
   git clone [repository-url]
   ```

2. Navigate to the project directory
   ```
   cd FormGenerator
   ```

3. Restore the required NuGet packages
   ```
   dotnet restore
   ```

4. Build and run the application
   ```
   dotnet run
   ```

5. Open your web browser and navigate to:
   - http://localhost:5073 (HTTP)

The exact ports may vary if they're already in use. Check the console output after running `dotnet run` for the actual URLs (example: Now listening on: https://localhost:XXXXX).

## How It Works

1. The application loads a JSON form definition from the `wwwroot` folder
2. Forms are generated dynamically based on the JSON structure
3. Validation rules are applied based on field configuration
4. Upon submission, form data is converted to JSON and displayed

## Project Structure

The application follows a structured architecture with clear separation of concerns:

### Core Components

- **DynamicForm.razor**: The main page component that orchestrates form rendering and handles submission
- **MainLayout.razor**: Defines the application shell with navigation and layout structure
- **AboutDialog.razor**: Information dialog showing application details
- **FormSubmissionDialog.razor**: Dialog to display submitted form data

### Form Field Components

- **InputFieldsSection.razor**: Renders text, email, and number input fields
- **DropdownFieldsSection.razor**: Renders dropdown/select fields
- **CheckboxFieldsSection.razor**: Renders checkbox fields
- **FormValidator.razor**: Custom validation component that integrates with Blazor's EditContext

### Utility Components

- **ErrorHandling.razor**: Error display component with expandable details
- **FormDataDisplay.razor**: Component to display formatted form data

### Models

- **FormModels.cs**: Core data models for form fields and validation
- **JsonModels.cs**: Models for deserializing JSON configuration

### Services

- **FormGenerationService**: Converts JSON definitions into FormModel objects
  - Parses field types and properties
  - Maps JSON properties to appropriate field models
  - Generates guaranteed unique IDs for each field to prevent reference issues

- **FormStateService**: Manages form state throughout the application lifecycle
  - Tracks current and original form state
  - Handles form initialization and reset operations
  - Manages numeric field values separately for MudBlazor components
    - This separate management is necessary because MudBlazor's MudNumericField component works with typed numeric values (int?, decimal?, etc.), while the form model stores values as strings for consistency
    - The NumericValues dictionary creates a bridge between the typed values needed by the UI components and the string values in the data model
    - This approach solves type conversion issues and prevents loss of data when switching between string and numeric representations

- **FormValidationService**: Implements validation logic for different field types
  - Provides validators for email and other specialized fields
  - Determines if fields have validation errors

- **FormDataProcessingService**: Processes submitted form data
  - Formats field values appropriately (e.g., numbers, booleans)
  - Converts form data to formatted JSON output

- **JsonLoaderService**: Loads JSON configuration files
  - Handles HTTP requests to retrieve files from wwwroot

### What Makes It Work

The application leverages several key Blazor and .NET features:

1. **Dependency Injection**: Services are injected into components, making the code modular and testable

2. **Component Model**: Blazor's component model allows for reusable UI pieces (like field sections)

3. **Two-way Binding**: `@bind-Value` syntax creates two-way data binding between UI and models

4. **EditContext**: Blazor's form validation uses EditContext to track validation state

5. **MudBlazor Components**: Pre-built Material Design components accelerate development

6. **Service Pattern**: Business logic is encapsulated in services with clear interfaces

7. **Data Flow**:
   - JSON → JsonLoaderService → FormGenerationService → FormModel
   - FormModel → Razor Components → UI
   - User Input → FormValidationService → Validation Results
   - Form Submission → FormDataProcessingService → JSON Output

## Form Configuration

The application uses JSON files to define form structure. By default, it loads `formConfig.json`, but you can modify the file path in `DynamicForm.razor`:

```csharp
/// <summary>
/// Path to JSON form definition
/// </summary>
private const string JsonFilePath = "formConfig.json";
```

Change this constant to use a different configuration file. NOTE: 'expandedFormConfig.json' is included in wwwroot. This file contains additional elements to test the generation of a larger form.

### JSON Structure

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
      "values": ["Tech", "Production", "Health"],
      "required": true
    }
  ]
}
```

## Development Journey and Challenges

This project took ~16 hours to complete. As my first Blazor application, it served as both a learning opportunity and a practical implementation of a dynamic form system.

### Key Challenges

1. **Form Validation**: Implementing validation for dynamically generated fields was particularly challenging. Blazor's `EditContext` and validation system required custom approaches to handle dynamic fields correctly.

2. **State Management**: Maintaining form state across component lifecycles, especially for numeric fields, required careful design of the state service.

3. **Learning Curve**: Blazor has a unique component model and lifecycle that differs from other web frameworks. Understanding concepts like cascading parameters, render fragments, and two-way binding took significant research.

4. **Component Architecture**: Designing a modular system that could be easily extended while maintaining clean separation of concerns required several architectural revisions.

5. **MudBlazor Integration**: While MudBlazor provides excellent UI components, integrating them with dynamic validation required custom solutions.

6. **Numeric Field Binding**: A particular challenge was ensuring that numeric fields maintained independent values when multiple instances existed on the same form. This required:
   - Implementing a robust ID generation system that guarantees uniqueness
   - Carefully managing the binding between MudNumericField components and the underlying data model
   - Using string-based references rather than object references to prevent unintended state sharing

## Future Improvements

Several areas could be enhanced in future iterations:

1. **Additional Field Types**: Support for date pickers, time inputs, rich text editors, and file uploads.

2. **Form Builder UI**: A visual interface to create and modify form definitions without editing JSON.

3. **Form Sections**: Group related fields into collapsible sections for better organization.

4. **Conditional Logic**: Show/hide fields based on values of other fields.

5. **Multi-step Forms**: Support for wizard-like multi-page forms with navigation.

6. **Server Integration**: API endpoints for saving and loading form data.

7. **Form Templates**: Save and reuse common form configurations.

8. **Performance Optimization**: Minimize re-renders and improve component lifecycle management.

9. **Enhanced Debugging**: More comprehensive logging to track component lifecycles and data flow.

## Known Issues and Solutions

### Resolved Issues

1. **Numeric Field Value Sharing**: Previously, changing one numeric field would update all numeric fields to the same value. This was resolved by:
   - Implementing a more robust field ID generation system in `FormGenerationService.cs`
   - Modifying the numeric field binding approach in `InputFieldsSection.razor` to use string IDs directly
   - Adding safer value retrieval and update methods that prevent unintended state sharing

2. **Compiler Warnings**: Fixed various nullability and validation-related compiler warnings in the form validation components.

3. **MudBlazor Component Issues**: Resolved issues with MudExpansionPanel attribute binding that were causing analyzer warnings.

## Extending the Project

### Adding New Field Types

To add a new field type (like radio buttons):

1. **Update the Models**:
   - Add a new model class `RadioButtonField.cs` extending the appropriate interfaces
   - Update `FormModel.cs` to include the new field type

2. **Update Form Generation**:
   - Modify `FormGenerationService.cs` to handle the new field type in JSON

3. **Create UI Component**:
   - Add a new component `RadioButtonFieldsSection.razor`
   - Implement the rendering and binding logic

4. **Update Validation**:
   - Add validation rules for the new field type in `FormValidationService.cs`
   - Create a custom validation attribute if needed

5. **Update JSON Processing**:
   - Modify `FormDataProcessingService.cs` to handle the new field type in output

### Example: Adding Radio Buttons

```csharp
// 1. Add to FormModel.cs
public List<RadioButtonField> RadioButtonFields { get; set; } = new();

// 2. Create RadioButtonField.cs
public class RadioButtonField
{
    public string Id { get; set; }
    public string Label { get; set; }
    public bool IsRequired { get; set; }
    public string SelectedValue { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
}

// 3. Update FormGenerationService.cs to handle "radio" type
case "radio":
    formModel.RadioButtonFields.Add(new RadioButtonField
    {
        Id = GenerateUniqueFieldId("radio"),  // Use the new unique ID generation method
        Label = field.Label ?? "Unnamed Radio Group",
        Options = field.Values ?? new List<string>(),
        IsRequired = field.Required
    });
    break;
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) - Web framework
- [MudBlazor](https://mudblazor.com/) - Material Design components