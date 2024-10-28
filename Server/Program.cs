using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(silo =>
    {
        silo.UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault()
            .ConfigureLogging(logging => logging
                .SetMinimumLevel(LogLevel.Error)
                .AddConsole());
    })
    .UseConsoleLifetime();

using var host = builder.Build();

await host.RunAsync();