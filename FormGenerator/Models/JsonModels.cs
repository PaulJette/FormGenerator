namespace FormGenerator.Models;

// Classes to represent the JSON structure
public class FormDefinition
{
    public string Title { get; set; }
    public List<FormFieldDefinition> Fields { get; set; }
}

public class FormFieldDefinition
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Label { get; set; }
    public bool Required { get; set; }
    public string DefaultValue { get; set; }
    public List<string> Values { get; set; }
    public int? Min { get; set; }
    public int? Max { get; set; }
}

