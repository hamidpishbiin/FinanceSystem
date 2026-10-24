using FinanceSystem.Domain.Bills.Exceptions;

namespace FinanceSystem.Domain.Bills.Services;

public class BillEligibilityService(IBillRepository repository)
{
    public async Task<Bill> GetBill(long billId, long userId, CancellationToken cancellationToken)
    {
        var bill = await repository.GetById(billId, cancellationToken);

        Guard<BillNotFoundException>.AgainstNull(bill);

        Guard<InvalidUserBillException>.IsTrue(bill!.UserId != userId);

        return bill!;
    }
}
