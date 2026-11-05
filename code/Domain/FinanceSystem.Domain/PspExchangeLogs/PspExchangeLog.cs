using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PspExchangeLogs.Enums;
using FinanceSystem.Domain.PspExchangeLogs.Exceptions;

namespace FinanceSystem.Domain.PspExchangeLogs;

public class PspExchangeLog : EntityBase<long>
{
    public PspExchangeStep Step { get; private set; }
    public long ReferenceId { get; private set; }
    public string? RequestBody { get; private set; }
    public string? ResponseBody { get; private set; }
    public int? HttpStatus { get; private set; }
    public int? DurationMs { get; private set; }

    private PspExchangeLog() { }

    private PspExchangeLog(
        PspExchangeStep step,
        long referenceId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        Guard<InvalidReferenceIdException>.IsTrue(referenceId <= 0);
        Guard<InvalidHttpStatusException>.IsTrue(httpStatus is < 100 or > 599);
        Guard<InvalidDurationException>.IsTrue(durationMs < 0);

        Step = step;
        ReferenceId = referenceId;
        RequestBody = requestBody;
        ResponseBody = responseBody;
        HttpStatus = httpStatus;
        DurationMs = durationMs;
    }

    public static PspExchangeLog CreateTokenLog(
        long referenceId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        return new PspExchangeLog(
            PspExchangeStep.Token,
            referenceId,
            requestBody,
            responseBody,
            httpStatus,
            durationMs);
    }

    public static PspExchangeLog CreateCallbackLog(
        long referenceId,
        string? requestBody)
    {
        return new PspExchangeLog(
            PspExchangeStep.Callback,
            referenceId,
            requestBody,
            null,
            null,
            null);
    }

    public static PspExchangeLog CreateVerificationLog(
        long referenceId,
        string? requestBody,
        string? responseBody,
        int? httpStatus,
        int? durationMs)
    {
        return new PspExchangeLog(
            PspExchangeStep.Verification,
            referenceId,
            requestBody,
            responseBody,
            httpStatus,
            durationMs);
    }
}


