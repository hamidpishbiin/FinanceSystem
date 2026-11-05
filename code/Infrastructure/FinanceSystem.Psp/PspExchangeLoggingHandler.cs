using System.Diagnostics;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.PspExchangeLogs;
using FinanceSystem.Domain.PspExchangeLogs.Enums;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core;

namespace FinanceSystem.Psp;

public class PspExchangeLoggingHandler(
    IServiceScopeFactory scopeFactory,
    ILoggerService loggerService)
    : DelegatingHandler
{
    override protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var step = GetRequired(request, PspExchangeContext.Step);
        var paymentId = GetRequired(request, PspExchangeContext.PaymentId);

        var requestBody = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync(cancellationToken);

        var stopWatch = Stopwatch.StartNew();

        HttpResponseMessage? response = null;

        try
        {
            response = await base.SendAsync(request, cancellationToken);
            return response;
        }
        finally
        {
            stopWatch.Stop();

            var responseBody = response?.Content is null
                ? null
                : await response.Content.ReadAsStringAsync(cancellationToken);

            await WriteAsync(() => step switch
            {
                PspExchangeStep.Token => PspExchangeLog.CreateTokenLog(
                    paymentId,
                    requestBody,
                    responseBody,
                    (int?)response?.StatusCode,
                    (int)stopWatch.ElapsedMilliseconds),
                PspExchangeStep.Verification => PspExchangeLog.CreateVerificationLog(
                    paymentId,
                    requestBody,
                    responseBody,
                    (int?)response?.StatusCode,
                    (int)stopWatch.ElapsedMilliseconds),
                PspExchangeStep.Reverse => PspExchangeLog.CreateReverseLog(
                    paymentId,
                    requestBody,
                    responseBody,
                    (int?)response?.StatusCode,
                    (int)stopWatch.ElapsedMilliseconds),
                _ => throw new ArgumentOutOfRangeException($"Invalid step {step}")
            });
        }
    }

    private async Task WriteAsync(Func<PspExchangeLog> createLog)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            var repository = scope.ServiceProvider.GetRequiredService<IPspExchangeLogRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await repository.AddAsync(createLog(), CancellationToken.None);
            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            loggerService.Error(ex, "Writing PspExchangeLogs Failed");
        }
    }

    private static T GetRequired<T>(HttpRequestMessage request, HttpRequestOptionsKey<T> key)
    {
        return request.Options.TryGetValue(key, out var value)
            ? value
            : throw new InvalidOperationException($"PSP request is missing key '{key.Key} in its options'");
    }
}
