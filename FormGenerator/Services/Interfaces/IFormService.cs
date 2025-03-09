namespace FormGenerator.Services.Interfaces;

using FormGenerator.Models;

public interface IFormService
{
    void GenerateForm(FormModel model);
    bool ValidateForm(FormModel model);
    void ProcessForm(FormModel model);
}

