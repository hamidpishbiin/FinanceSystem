using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PspPaymentDetails;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using NSubstitute;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.PspPaymentDetailUnitTests;

public class PspPaymentDetailBuilder
{
    public const PspCode DefaultPspCode = PspCode.Saman;
    public const decimal DefaultRequestAmountRial = 1234123;
    public const long DefaultTargetAccountId = 12;

    private PspCode PspCode { get; set; } = DefaultPspCode;
    private Money RequestAmount { get; set; } = new(DefaultRequestAmountRial);
    private long TargetAccountId { get; set; } = DefaultTargetAccountId;

    public IEventPublisher EventPublisher { get; private set; } = Substitute.For<IEventPublisher>();

    public async Task<PspPaymentDetail> Build()
    {
        return await PspPaymentDetail.Create(PspCode, TargetAccountId, RequestAmount, EventPublisher);
    }

    public PspPaymentDetailBuilder WithPspCode(PspCode pspCode)
    {
        PspCode = pspCode;
        return this;
    }

    public PspPaymentDetailBuilder WithEventPublisher(IEventPublisher? eventPublisher)
    {
        EventPublisher = eventPublisher!;
        return this;
    }

    public PspPaymentDetailBuilder WithRequestAmount(Money requestAmount)
    {
        RequestAmount = requestAmount;
        return this;
    }

    public PspPaymentDetailBuilder WithTargetAccountId(long targetAccountId)
    {
        TargetAccountId = targetAccountId;
        return this;
    }

}
