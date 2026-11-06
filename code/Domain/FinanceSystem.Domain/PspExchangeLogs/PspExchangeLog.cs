using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PspExchangeLogs.Enums;
using FinanceSystem.Domain.PspExchangeLogs.Exceptions;

namespace FinanceSystem.Domain.PspExchangeLogs;

public class PspExchangeLog : EntityBase<long>
{
    public PspExchangeStep Step { get; private set; }
    public long PaymentId { get; private set; }
    public string? RequestBody { get; private set; }
    public string? ResponseBody { get; private set; }
    public int? HttpStatus { get; private set; }
    public int? DurationMs { get; private set; }

    private PspExchangeLog() { }

    private PspExchangeLog(
        PspExchangeStep step,
        long paymentId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        Guard<InvalidReferenceIdException>.IsTrue(paymentId <= 0);
        Guard<InvalidHttpStatusException>.IsTrue(httpStatus is < 100 or > 599);
        Guard<InvalidDurationException>.IsTrue(durationMs < 0);

        Step = step;
        PaymentId = paymentId;
        RequestBody = requestBody;
        ResponseBody = responseBody;
        HttpStatus = httpStatus;
        DurationMs = durationMs;
    }

    public static PspExchangeLog CreateTokenLog(
        long paymentId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        return new PspExchangeLog(
            PspExchangeStep.Token,
            paymentId,
            requestBody,
            responseBody,
            httpStatus,
            durationMs);
    }

    public static PspExchangeLog CreateCallbackLog(
        long paymentId,
        string? requestBody)
    {
        return new PspExchangeLog(
            PspExchangeStep.Callback,
            paymentId,
            requestBody,
            null,
            null,
            null);
    }

    public static PspExchangeLog CreateVerificationLog(
        long paymentId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        return new PspExchangeLog(
            PspExchangeStep.Verification,
            paymentId,
            requestBody,
            responseBody,
            httpStatus,
            durationMs);
    }

    public static PspExchangeLog CreateReverseLog(
        long paymentId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        return new PspExchangeLog(
            PspExchangeStep.Reverse,
            paymentId,
            requestBody,
            responseBody,
            httpStatus,
            durationMs);
    }
}


