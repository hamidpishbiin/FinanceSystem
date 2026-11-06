using FinanceSystem.Application.Contracts.Gateways;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Psp;

public static class PspHttpClientRegistration
{
    public static IServiceCollection AddPspHttpClients(this IServiceCollection services)
    {
        services.AddTransient<PspExchangeLoggingHandler>();

        services.AddHttpClient(nameof(PspCode.Saman),
                (provider, client) =>
                {
                    Configure(client, provider, options => options.Saman);
                })
            .AddHttpMessageHandler<PspExchangeLoggingHandler>();

        services.AddHttpClient(nameof(PspCode.BehPardakht),
                (provider, client) =>
                {
                    Configure(client, provider, options => options.BehPardakht);
                })
            .AddHttpMessageHandler<PspExchangeLoggingHandler>();

        return services;
    }

    private static void Configure(HttpClient client, IServiceProvider provider, Func<PspOptions, PspSettings> select)
    {
        var settings = select(provider.GetRequiredService<IOptionsMonitor<PspOptions>>().CurrentValue);

        client.BaseAddress = new Uri(settings.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds);
    }
}
