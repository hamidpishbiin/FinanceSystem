using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.RequestsToPay;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using NSubstitute;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.RequestToPayUnitTests;

public class RequestToPayBuilder
{
    public const PspCode DefaultPspCode = PspCode.Saman;
    public const decimal DefaultRequestAmountRial = 1234123;
    public const long DefaultTargetAccountId = 12;

    private PspCode PspCode { get; set; } = DefaultPspCode;
    private Money RequestAmount { get; set; } = new(DefaultRequestAmountRial);
    private long TargetAccountId { get; set; } = DefaultTargetAccountId;

    public IEventPublisher EventPublisher { get; private set; } = Substitute.For<IEventPublisher>();

    public async Task<RequestToPay> Build()
    {
        return await RequestToPay.Create(PspCode, TargetAccountId, RequestAmount, EventPublisher);
    }

    public RequestToPayBuilder WithPspCode(PspCode pspCode)
    {
        PspCode = pspCode;
        return this;
    }

    public RequestToPayBuilder WithEventPublisher(IEventPublisher? eventPublisher)
    {
        EventPublisher = eventPublisher!;
        return this;
    }

    public RequestToPayBuilder WithRequestAmount(Money requestAmount)
    {
        RequestAmount = requestAmount;
        return this;
    }

    public RequestToPayBuilder WithTargetAccountId(long targetAccountId)
    {
        TargetAccountId = targetAccountId;
        return this;
    }

}
