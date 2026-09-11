using FinanceSystem.Domain.BankPaymentDetails;
using FinanceSystem.Domain.BankPaymentDetails.Enums;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;

namespace FinanceSystem.Domain.Tests.Unit.BankPaymentDetailUnitTests;

public class BankPaymentDetailBuilder
{
    public const string DefaultGuidString = "248e3600-cf20-4ca3-84f0-b76a92277e73";
    public const decimal DefaultRequestAmountRial = 1234123;
    public const decimal DefaultRedirectedAmountRial = 8970987;
    public const long DefaultTargetAccountId = 12;
    public const string DefaultAuthority = "alksjdhflasdkjfh";
    // public const string DefaultRRN = "02934875";
    // public const string DefaultRefNum = "8234756";
    // public const string DefaultTraceNumber = "02934875";
    // public const string DefaultMaskedPan = "19823764";
    // public const int DefaultResultCode = 0;
    // public const string DefaultRawCallback = "alsjdfhlasdkjhf";
    // public static readonly DateTimeOffset DefaultVerifiedAtUtc = new(2026, 09, 10, 0, 0, 0, TimeSpan.Zero);

    private Guid PspId { get; set; } = Guid.Parse(DefaultGuidString);
    private BankPaymentStatus Status { get; set; }
    private Money RequestAmount { get; set; } = new(DefaultRequestAmountRial, Currency.Rial);
    private Money? RedirectedAmount { get; set; }
    private long TargetAccountId { get; set; } = DefaultTargetAccountId;
    private string Authority { get; set; } = DefaultAuthority;
    private string? RRN { get; set; }
    private string? RefNum { get; set; }
    private string? TraceNumber { get; set; }
    private string? MaskedPan { get; set; }
    private int? ResultCode { get; set; }
    private string? RawCallback { get; set; }
    private DateTimeOffset? VerifiedAtUtc { get; set; }

    public async Task<BankPaymentDetail> Build()
    {
        return await BankPaymentDetail.Create(PspId, TargetAccountId, RequestAmount, Authority);
    }

    public BankPaymentDetailBuilder WithPspId(Guid pspId)
    {
        PspId = pspId;
        return this;
    }

    public BankPaymentDetailBuilder WithStatus(BankPaymentStatus status)
    {
        Status = status;
        return this;
    }

    public BankPaymentDetailBuilder WithRequestAmount(Money requestAmount)
    {
        RequestAmount = requestAmount;
        return this;
    }

    public BankPaymentDetailBuilder WithRedirectAmount(Money? redirectAmount)
    {
        RedirectedAmount = redirectAmount!;
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

    public BankPaymentDetailBuilder WithRRN(string? rrn)
    {
        RRN = rrn;
        return this;
    }

    public BankPaymentDetailBuilder WithRefNum(string? refNum)
    {
        RefNum = refNum;
        return this;
    }

    public BankPaymentDetailBuilder WithTraceNumber(string? traceNumber)
    {
        TraceNumber = traceNumber;
        return this;
    }

    public BankPaymentDetailBuilder WithMaskedPan(string? maskedPan)
    {
        MaskedPan = maskedPan;
        return this;
    }

    public BankPaymentDetailBuilder WithResultCode(int? resultCode)
    {
        ResultCode = resultCode;
        return this;
    }

    public BankPaymentDetailBuilder WithRawCallback(string? rawCallback)
    {
        RawCallback = rawCallback;
        return this;
    }

    public BankPaymentDetailBuilder WithVerifiedAtUtc(DateTimeOffset? verifiedAtUtc)
    {
        VerifiedAtUtc = verifiedAtUtc;
        return this;
    }
}
