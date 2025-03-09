using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FormGenerator;
using FormGenerator.Services.Implementations;
using FormGenerator.Services.Interfaces;
using MudBlazor.Services;
using FormGenerator.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient for API access
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register MudBlazor services
builder.Services.AddMudServices();

// Register our services with interfaces
builder.Services.AddScoped<IFormGenerationService, FormGenerationService>();
builder.Services.AddScoped<IJsonLoaderService, JsonLoaderService>();
builder.Services.AddScoped<IFormStateService, FormStateService>();
builder.Services.AddScoped<IFormValidationService, FormValidationService>();
builder.Services.AddScoped<IFormDataProcessingService, FormDataProcessingService>();
builder.Services.AddLogging();

await builder.Build().RunAsync();