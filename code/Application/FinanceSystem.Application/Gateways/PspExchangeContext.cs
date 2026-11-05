using FinanceSystem.Domain.PspExchangeLogs.Enums;

namespace FinanceSystem.Application.Gateways;

public static class PspExchangeContext
{
    public static readonly HttpRequestOptionsKey<PspExchangeStep> Step = new(nameof(PspExchangeStep));
    public static readonly HttpRequestOptionsKey<long> PaymentId = new(nameof(PaymentId));

    public static HttpRequestMessage WithExchangeContext(
        this HttpRequestMessage message,
        PspExchangeStep step,
        long paymentId)
    {
        message.Options.Set(Step, step);
        message.Options.Set(PaymentId, paymentId);

        return message;
    }
}
