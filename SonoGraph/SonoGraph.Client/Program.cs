using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SonoGraph.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<AudioPlayerService>();
builder.Services.AddScoped<StorageService>();
builder.Services.AddScoped<SoundService>();


var app = builder.Build();

await app.RunAsync();
