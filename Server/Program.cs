using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(silo =>
    {
        silo.Configure<GrainCollectionOptions>(x => { x.CollectionAge = TimeSpan.FromSeconds(61); })
            .UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault()
            .ConfigureLogging(logging => logging
                .SetMinimumLevel(LogLevel.Error)
                .AddConsole());
    })
    .UseConsoleLifetime();

using var host = builder.Build();

await host.RunAsync();