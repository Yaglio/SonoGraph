using SonoGraph.Client.Pages;
using SonoGraph.Client.Services;
using SonoGraph.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<AudioPlayerService>();
builder.Services.AddScoped<StorageService>();
builder.Services.AddScoped<SoundService>();
builder.Services.AddScoped<AudioEditorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}



app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SonoGraph.Client._Imports).Assembly);

app.Run();
