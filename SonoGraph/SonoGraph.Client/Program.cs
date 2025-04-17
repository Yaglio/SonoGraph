using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SonoGraph.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<AudioPlayerService>();

builder.Services.AddSingleton<VolumeService>();

await builder.Build().RunAsync();
