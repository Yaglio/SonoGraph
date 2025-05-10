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

var app = builder.Build();

// Initialize scoped services
using (var scope = app.Services.CreateScope())
{
    var initializableServices = scope.ServiceProvider.GetServices<IInitializableService>();
    foreach (var service in initializableServices)
    {
        await service.Initialize();
    }
}

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
