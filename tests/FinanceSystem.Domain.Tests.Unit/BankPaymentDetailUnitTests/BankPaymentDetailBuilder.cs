using FinanceSystem.Domain.BankPaymentDetails;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.BankPaymentDetailUnitTests;

public class BankPaymentDetailBuilder
{
    public const string DefaultGuidString = "248e3600-cf20-4ca3-84f0-b76a92277e73";
    public const decimal DefaultRequestAmountRial = 1234123;
    public const long DefaultTargetAccountId = 12;
    public const string DefaultAuthority = "alksjdhflasdkjfh";

    private Guid PspId { get; set; } = Guid.Parse(DefaultGuidString);
    private Money RequestAmount { get; set; } = new(DefaultRequestAmountRial, Currency.Rial);
    private long TargetAccountId { get; set; } = DefaultTargetAccountId;
    private string Authority { get; set; } = DefaultAuthority;

    public async Task<BankPaymentDetail> Build()
    {
        return await BankPaymentDetail.Create(PspId, TargetAccountId, RequestAmount, Authority);
    }

    public BankPaymentDetailBuilder WithPspId(Guid pspId)
    {
        PspId = pspId;
        return this;
    }

    public BankPaymentDetailBuilder WithRequestAmount(Money requestAmount)
    {
        RequestAmount = requestAmount;
        return this;
    }

    public BankPaymentDetailBuilder WithTargetAccountId(long targetAccountId)
    {
        TargetAccountId = targetAccountId;
        return this;
    }

    public BankPaymentDetailBuilder WithAuthority(string? authority)
    {
        Authority = authority!;
        return this;
    }
}
