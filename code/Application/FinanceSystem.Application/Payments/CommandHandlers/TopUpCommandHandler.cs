using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Contracts.Payments.Events;
using FinanceSystem.Application.Gateways;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;
using FinanceSystem.Domain.Users;
using FinanceSystem.Domain.Users.Exceptions;
using Microsoft.Extensions.Options;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class TopUpCommandHandler(
    IPaymentRepository paymentRepository,
    IEventPublisher publisher,
    IEventListener listener,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IGuidGenerator guidGenerator,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    FinanceAccountEligibilityService financeAccountEligibilityService,
    IOptions<PspOptions> pspOptions)
    : PaymentCommandHandler<TopUpCommand>(paymentRepository, publisher, listener)
{
    private const string _serviceId = "FinanceSystem";

    public override async Task Handle(TopUpCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);

        Guard<UserNotFoundException>.AgainstNull(user);

        await financeAccountEligibilityService.GetEligibleAccountForTopUpAsync(user!.Id, cancellationToken);

        var amount = new Money(command.Amount);

        var psp = await pspSelector.SelectAsync((PspCode)command.PspCode!.Value, cancellationToken);

        var payment = Payment.CreateTopUpPayment(
            amount,
            user.Id,
            _serviceId,
            guidGenerator.New().ToString(),
            psp.Code,
            Publisher);

        await Repository.AddAsync(payment);

        await unitOfWork.SaveChangesAsync();

        var pspGateway = pspGatewayFactory.Get(psp.Code);

        var pspTokenRequest = new PspTokenRequest()
        {
            Amount = amount.Value,
            ReservationNumber = payment.Id,
            CallbackUrl = pspOptions.Value.For(psp.Code).TopUpCallBackUrl
        };

        var paymentTokenResponse = await pspGateway.RequestPaymentTokenAsync(pspTokenRequest, cancellationToken);

        if (paymentTokenResponse.IsSuccess)
        {
            await payment.MarkTokenReceived(paymentTokenResponse.Token);
            await Publisher.Publish(new PspHandoffReady(paymentTokenResponse.Handoff!));
        }
        else
        {
            await payment.MarkTokenRequestFailed(PaymentFailureReason.TokenRequestFailed);
        }
    }
}
