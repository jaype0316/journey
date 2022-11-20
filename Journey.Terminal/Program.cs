// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var host = CreateHostBuilder();
        await host.RunConsoleAsync();
        return Environment.ExitCode;
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        });
    }
}

public class Worker : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
