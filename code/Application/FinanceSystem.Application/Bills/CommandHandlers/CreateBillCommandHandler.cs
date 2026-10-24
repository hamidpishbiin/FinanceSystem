using FinanceSystem.Application.Contracts.Bills.Commands;
using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Contract.Bills;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public class CreateBillCommandHandler(
    IEventPublisher publisher,
    IEventListener listener,
    IBillRepository repository)
    : BillCommandHandler<CreateBillCommand>(repository, publisher, listener)
{
    public override async Task Handle(CreateBillCommand command, CancellationToken cancellationToken = default)
    {
        var bill = new Bill(command.ServiceId, command.ReferenceId, command.UserId, new Money(command.Amount));

        await Repository.AddAsync(bill, cancellationToken);

        await Publisher.Publish(new BillCreatedEvent(bill.Id));
    }
}
