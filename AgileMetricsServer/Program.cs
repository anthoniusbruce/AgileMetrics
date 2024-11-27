using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AdoAnalysisServerApis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IAdoAnalysisService, AdoAnalysisService>();

builder.Services.AddHttpClient(AdoAnalysisService.TrTaxOrganization, client =>
{
    client.BaseAddress = new Uri("https://analytics.dev.azure.com/tr-tax/TaxProf/_odata/v4.0-preview/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient(AdoAnalysisService.TrTaxDefaultPatagoniaOrganization, client =>
{
    client.BaseAddress = new Uri("https://analytics.dev.azure.com/tr-tax-default/Patagonia/_odata/v4.0-preview/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient(AdoAnalysisService.TrTaxDefaultEflexwareOrganization, client =>
{
    client.BaseAddress = new Uri("https://analytics.dev.azure.com/tr-tax-default/eFlexware/_odata/v4.0-preview/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

