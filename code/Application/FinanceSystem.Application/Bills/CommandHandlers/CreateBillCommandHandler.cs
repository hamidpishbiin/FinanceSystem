using FinanceSystem.Application.Contracts.Bills.Commands;
using FinanceSystem.Application.Contracts.Payments.Command;
using FinanceSystem.Domain.Bills;
using FinanceSystem.Domain.Contract.Bills;
using FinanceSystem.Domain.FinanceAccounts.DomainServices;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Application.Bills.CommandHandlers;

public class CreateBillCommandHandler : BillCommandHandler<CreateBillCommand, BillCreatedEvent>
{
    private readonly FinanceAccountEligibilityService _financeAccountEligibilityService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBillCommandHandler(
        IEventPublisher publisher,
        IEventListener listener,
        IEventHandler<BillCreatedEvent> eventHandler,
        IBillRepository repository,
        FinanceAccountEligibilityService financeAccountEligibilityService,
        IUnitOfWork unitOfWork)
        : base(publisher, listener, eventHandler, repository)
    {
        _financeAccountEligibilityService = financeAccountEligibilityService;
        _unitOfWork = unitOfWork;
    }

    public override async Task Execute(CreateBillCommand command, CancellationToken cancellationToken = default)
    {
        var financeAccount = await _financeAccountEligibilityService.GetBillTargetAccountAsync(command.UserId, cancellationToken);

        var bill = new Bill(command.ServiceId, command.ReferenceId, financeAccount.Id, new Money(command.Amount));

        await Repository.AddAsync(bill, cancellationToken);

        await _unitOfWork.SaveChangesAsync();

        await Publisher.Publish(new BillCreatedEvent(bill.Id));
    }
}
