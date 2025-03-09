namespace FormGenerator.Services.Interfaces;

/// <summary>
/// Interface for service that loads JSON files from wwwroot
/// </summary>
public interface IJsonLoaderService
{
    /// <summary>
    /// Loads a JSON file from the wwwroot folder
    /// </summary>
    /// <param name="fileName">The name of the file to load (relative to wwwroot)</param>
    /// <returns>The file content as a string</returns>
    Task<string> LoadJsonFromWwwrootAsync(string fileName);
}
