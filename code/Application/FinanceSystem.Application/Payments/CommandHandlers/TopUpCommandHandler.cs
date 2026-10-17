using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.FinanceAccounts.Exceptions;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;
using FinanceSystem.Domain.PaymentServiceProviders.Services;
using FinanceSystem.Domain.Users;
using FinanceSystem.Domain.Users.Exceptions;

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
    FinanceAccountEligibilityService financeAccountEligibilityService)
    : PaymentCommandHandler<TopUpCommand>(paymentRepository, publisher, listener)
{
    private const string _serviceId = "FinanceSystem";

    public override async Task Handle(TopUpCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);

        Guard<UserNotFoundException>.AgainstNull(user);

        var userFinanceAccount = await financeAccountEligibilityService.GetTopUpTargetAccountAsync(user!.Id, cancellationToken);

        Guard<FinanceAccountNotFoundException>.AgainstNull(userFinanceAccount);

        var amount = new Money(command.Amount);

        var psp = await pspSelector.SelectAsync(command.PspCode!.Value, cancellationToken);

        var payment = Payment.CreateTopUpPayment(
            amount,
            userFinanceAccount!.Id,
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
        };

        var paymentTokenResponse = await pspGateway.RequestPaymentTokenAsync(pspTokenRequest, cancellationToken);

        if (paymentTokenResponse.IsSuccess)
        {
            await payment.MarkTokenReceived(
                paymentTokenResponse.Token,
                paymentTokenResponse.IpgUrl);
        }
        else
        {
            await payment.MarkTokenRequestFailed(
                PaymentFailureReason.Unknown,
                paymentTokenResponse.Status,
                paymentTokenResponse.StatusCode,
                paymentTokenResponse.Description);
        }
    }
}
