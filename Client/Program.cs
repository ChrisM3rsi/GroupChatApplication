using Grains;
using Grains.Impl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();

using var host = builder.Build();
await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();

var me = client.GetGrain<IPersonGrain>("Chris");

var observer = client.CreateObjectReference<IMessageObserver>(new MessageObserver());

await me.JoinGroup("petsGroup", observer);
await me.SendMessage("My rabbit is awesome");




await Task.Delay(10000);
await host.StopAsync();
