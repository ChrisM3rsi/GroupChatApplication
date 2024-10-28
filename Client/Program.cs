using Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client => { client.UseLocalhostClustering(); })
    .ConfigureLogging(logging => logging
        .SetMinimumLevel(LogLevel.Error)
        .AddConsole())
    .UseConsoleLifetime();

using var host = builder.Build();
await host.StartAsync();

var consoleService = new ConsoleService();
_ = consoleService.StartConsole(TimeSpan.FromMilliseconds(1000));

var client = host.Services.GetRequiredService<IClusterClient>();

var chatService = new ChatService(
    client,
    m => consoleService.AppendToOutput(m));

consoleService.SubscribeToChatService(chatService);


await consoleService.HandleInput(async m  => await chatService.HandleInput(m));


await host.StopAsync();
