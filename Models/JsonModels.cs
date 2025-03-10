namespace FormGenerator.Models;

// Classes to represent the JSON structure
public class FormDefinition
{
    public string Title { get; set; } = string.Empty;
    public List<FormFieldDefinition> Fields { get; set; } = [];
}

public class FormFieldDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public bool Required { get; set; }
    public string DefaultValue { get; set; } = string.Empty;
    public List<string> Values { get; set; } = [];
    public int? Min { get; set; }
    public int? Max { get; set; }
}

