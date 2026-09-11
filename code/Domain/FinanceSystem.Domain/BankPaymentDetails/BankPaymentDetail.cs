using FinanceSystem.Domain.BankPaymentDetails.Exceptions;
using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.BankPaymentDetails.Enums;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using Shared.Domain.Exceptions;

namespace FinanceSystem.Domain.BankPaymentDetails;

public class BankPaymentDetail : EntityBase<long>, IAggregateRoot
{
    public IEventPublisher Publisher { get; set; } = default!;

    public Guid PspId { get; private set; }
    public BankPaymentStatus Status { get; private set; }
    public decimal RequestAmountRial { get; private set; }
    public decimal? RedirectedAmountRial { get; private set; }
    public long TargetAccountId { get; private set; }
    public string Authority { get; private set; } = default!;
    public string? RRN { get; private set; }
    public string? RefNum { get; private set; }
    public string? TraceNumber { get; private set; }
    public string? MaskedPan { get; private set; }
    public int? ResultCode { get; private set; }
    public string? RawCallback { get; private set; }
    public DateTimeOffset? VerifiedAtUtc { get; private set; }

    public PaymentServiceProvider? Psp { get; private set; }
    public Account? TargetAccount { get; private set; }

    private BankPaymentDetail()
    {
    }

    public static async Task<BankPaymentDetail> Create(
        Guid pspId,
        long targetAccountId,
        Money requestAmountRial,
        string authority)
    {
        Guard<InvalidPspIdException>.IsTrue(pspId == Guid.Empty);
        Guard<InvalidTargetAccountIdException>.IsTrue(targetAccountId <= 0);
        Guard<NullEntryException>.AgainstNull(requestAmountRial);
        Guard<InvalidRequestAmountException>.IsTrue(requestAmountRial.Value == 0);
        Guard<InvalidMoneyCurrencyException>.IsTrue(requestAmountRial.Currency != Currency.Rial);
        Guard<InvalidAuthorityException>.AgainstNullOrEmpty(authority);

        return new BankPaymentDetail()
        {
            PspId = pspId,
            Status = BankPaymentStatus.Initiated,
            RequestAmountRial = requestAmountRial.Value,
            TargetAccountId = targetAccountId,
            Authority = authority
        };
    }
}
