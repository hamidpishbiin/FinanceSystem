using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Application.Payments.Gateways;
using FinanceSystem.Domain.FinanceAccounts;
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
    IFinanceAccountRepository financeAccountRepository,
    IEventPublisher publisher,
    IEventListener listener,
    PspSelector pspSelector,
    IPspGatewayFactory pspGatewayFactory,
    IGuidGenerator guidGenerator,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository)
    : PaymentCommandHandler<TopUpCommand>(paymentRepository, publisher, listener)
{
    private const string _originServiceId = "FinanceSystem";
    private const string _externalTag = "TopUp";

    public override async Task Handle(TopUpCommand command, CancellationToken cancellationToken = default)
    {
        var userExist = await userRepository.ExistsAsync(command.UserId, cancellationToken);

        Guard<UserNotFoundException>.IsFalse(userExist);

        var userFinanceAccount = await financeAccountRepository.GetByUserId(command.UserId, cancellationToken);

        Guard<FinanceAccountNotFoundException>.AgainstNull(userFinanceAccount);

        var targetFinanceAccountId = await financeAccountRepository.GetCompanyWalletIdAsync(cancellationToken);

        Guard<SystemFinanceAccountNotFoundException>.IsTrue(targetFinanceAccountId is null);

        var amount = new Money(command.Amount);

        var psp = await pspSelector.SelectAsync((PspCode)command.PspCode, cancellationToken);

        var payment = await Payment.Create(
            PaymentPurpose.TopUp,
            PaymentChannel.Psp,
            amount,
            targetFinanceAccountId!.Value,
            userFinanceAccount!.Id,
            _originServiceId,
            guidGenerator.New().ToString(),
            _externalTag,
            psp.Code,
            Publisher);

        await Repository.AddAsync(payment);

        await unitOfWork.SaveChangesAsync();

        var pspGateway = pspGatewayFactory.Get(psp.Code);

        var pspTokenRequest = new PspTokenRequest()
        {
            Amount = amount.Value,
            ReferenceNumber = payment.Id,
        };

        var paymentTokenResponse = await pspGateway.RequestTokenAsync(pspTokenRequest, cancellationToken);

        if (!paymentTokenResponse.IsSuccess)
        {
            await payment.MarkTokenRequestFailed(
                paymentTokenResponse.FailureReason ?? PspFailureReason.Unknown,
                paymentTokenResponse.RawStatus,
                paymentTokenResponse.RawErrorCode,
                paymentTokenResponse.RawDescription);
        }
        else
        {
            await payment.MarkTokenReceived(
                paymentTokenResponse.PaymentToken,
                paymentTokenResponse.IpgUrl);
        }
    }
}
