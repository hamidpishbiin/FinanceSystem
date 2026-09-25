using FinanceSystem.Domain.PspPaymentDetails.Exceptions;
using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.Contract.PspPaymentDetails;
using FinanceSystem.Domain.PspPaymentDetails.Enums;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.PspPaymentDetails;

public class PspPaymentDetail : EntityBase<long>, IAggregateRoot
{
    public IEventPublisher Publisher { get; set; } = default!;

    public PspCode PspCode { get; private set; }
    public PspPaymentStatus Status { get; private set; }
    public decimal RequestAmountRial { get; private set; }
    public decimal? RedirectedAmountRial { get; private set; }
    public long TargetAccountId { get; private set; }
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
    public Account? TargetAccount { get; private set; }

    private PspPaymentDetail()
    {
    }

    public static async Task<PspPaymentDetail> Create(
        PspCode pspCode,
        long targetAccountId,
        Money amount,
        IEventPublisher eventPublisher)
    {
        Guard<InvalidPspCodeException>.IsFalse(Enum.IsDefined(pspCode));
        Guard<InvalidTargetAccountIdException>.IsTrue(targetAccountId <= 0);
        Guard<NullEntryException>.AgainstNull(amount);
        Guard<InvalidRequestAmountException>.IsTrue(amount.Value <= 0);
        Guard<NullEntryException>.AgainstNull(eventPublisher);

        return new PspPaymentDetail()
        {
            PspCode = pspCode,
            Status = PspPaymentStatus.Initiated,
            RequestAmountRial = amount.Value,
            TargetAccountId = targetAccountId,
            Publisher = eventPublisher
        };
    }

    public async Task MarkPaymentFailed(
        PspFailureReason failureReason,
        string? rawStatus,
        string? rawErrorCode,
        string? rawDescription)
    {
        Status = PspPaymentStatus.Failed;
        FailureReason = failureReason;
        RawStatus = rawStatus;
        RawErrorCode = rawErrorCode;
        RawDescription = rawDescription;

        await Publisher.Publish(new PspPaymentFailedEvent(Id));
    }

    public async Task MarkPaymentTokenReceived(string token, string ipgUrl)
    {
        Guard<InvalidTokenException>.AgainstNullOrEmpty(token);

        Token = token;
        Status = PspPaymentStatus.TokenReceived;

        await Publisher.Publish(new PspPaymentTokenReceivedEvent(Id, ipgUrl));
    }
}
