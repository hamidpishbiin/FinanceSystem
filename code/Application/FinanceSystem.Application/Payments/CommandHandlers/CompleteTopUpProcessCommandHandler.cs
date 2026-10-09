using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Payments;
using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.Payments.Exceptions;
using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.CommandHandlers;

public class CompleteTopUpProcessCommandHandler(
    IPaymentRepository repository,
    IEventPublisher publisher,
    IEventListener listener) : PaymentCommandHandler<CompleteTopUpProcessCommand>(repository, publisher, listener)
{
    public override async Task Handle(CompleteTopUpProcessCommand command, CancellationToken cancellationToken = default)
    {
        var payment = await Repository.GetByIdAsync(command.ReservationNumber, cancellationToken);

        Guard<PaymentNotFoundException>.AgainstNull(payment);

        payment!.Publisher = Publisher;

        Guard<PspCodeMismatchException>.IsTrue(payment.PspCode != (PspCode)command.PspCode);
        Guard<InvalidPaymentStateException>.IsTrue(payment.State != PaymentState.TokenReceived);

        if (!command.IsSuccess)
        {
            await payment.MarkFailed(command.RawStatus!, command.RawStatusCode);
            return;
        }

        var duplicatePayment = await Repository.GetByReferenceNumberAsync(command.ReferenceNumber!, cancellationToken);

        Guard<DuplicateReferenceNumberException>.IsTrue(duplicatePayment != null);

        // verify the transaction

        // update the payment in the database by the verification result

        // reverse if necessary

        // Add the result pages

        // Add logging

        // Add MarkFailed tests

        // Add a state-machine-like logic for states

        throw new NotImplementedException();
    }
}
