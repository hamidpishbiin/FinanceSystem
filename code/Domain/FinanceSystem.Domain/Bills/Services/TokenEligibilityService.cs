using FinanceSystem.Domain.Bills.Exceptions;

namespace FinanceSystem.Domain.Bills.Services;

public class TokenEligibilityService(IBillRepository repository)
{
    public async Task<Bill> GetBill(long token, long userId, CancellationToken cancellationToken)
    {
        var bill = await repository.GetById(token, cancellationToken);

        Guard<BillNotFoundException>.AgainstNull(bill);

        Guard<InvalidUserBillException>.IsFalse(bill!.Id == userId);

        return bill!;
    }
}
