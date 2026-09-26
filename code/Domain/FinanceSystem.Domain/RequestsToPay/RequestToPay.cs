using FinanceSystem.Domain.RequestsToPay.Exceptions;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.Contract.RequestsToPay;
using FinanceSystem.Domain.RequestsToPay.Enums;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.RequestsToPay;

public class RequestToPay : EntityBase<long>, IAggregateRoot
{
    public IEventPublisher Publisher { get; set; } = default!;

    public PspCode PspCode { get; private set; }
    public RequestToPayStatus Status { get; private set; }
    public decimal RequestAmount { get; private set; }
    public decimal? RedirectedAmount { get; private set; }
    public long TargetFinanceAccountId { get; private set; }
    public long ReferenceNumber { get; private set; }
    public string? Token { get; private set; }
    public string? RRN { get; private set; }
    public string? RefNum { get; private set; }
    public string? TraceNumber { get; private set; }
    public string? MaskedPan { get; private set; }
    public PspFailureReason? FailureReason { get; private set; }
    public string? RawStatus { get; private set; }
    public string? RawErrorCode { get; private set; }
    public string? RawDescription { get; private set; }
    public DateTimeOffset? VerifiedAtUtc { get; private set; }
    public PaymentServiceProvider? Psp { get; private set; }
    public FinanceAccount? TargetFinanceAccount { get; private set; }

    private RequestToPay()
    {
    }

    public RequestToPay(
        PspCode pspCode,
        long targetFinanceAccountId,
        Money amount,
        IEventPublisher eventPublisher)
    {
        Guard<InvalidPspCodeException>.IsFalse(Enum.IsDefined(pspCode));
        Guard<InvalidTargetFinanceAccountIdException>.IsTrue(targetFinanceAccountId <= 0);
        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidRequestAmountException>.IsTrue(amount.Value <= 0);
        Guard<NullEntryException>.AgainstNull(eventPublisher);

        PspCode = pspCode;
        Status = RequestToPayStatus.Initiated;
        RequestAmount = amount.Value;
        TargetFinanceAccountId = targetFinanceAccountId;
        Publisher = eventPublisher;
    }

    public async Task MarkTokenRequestFailed(
        PspFailureReason failureReason,
        string? rawStatus,
        string? rawErrorCode,
        string? rawDescription)
    {
        Status = RequestToPayStatus.Failed;
        FailureReason = failureReason;
        RawStatus = rawStatus;
        RawErrorCode = rawErrorCode;
        RawDescription = rawDescription;

        await Publisher.Publish(new RequestToPayFailedEvent(Id));
    }

    public async Task MarkTokenReceived(string token, string ipgUrl)
    {
        Guard<InvalidTokenException>.AgainstNullOrEmpty(token);

        Token = token;
        Status = RequestToPayStatus.TokenReceived;

        await Publisher.Publish(new RequestToPayTokenReceivedEvent(Id, ipgUrl));
    }
}
