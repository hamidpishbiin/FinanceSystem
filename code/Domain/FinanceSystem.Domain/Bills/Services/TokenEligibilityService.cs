using FinanceSystem.Domain.Bills.Exceptions;

namespace FinanceSystem.Domain.Bills.Services;

public class TokenEligibilityService(IBillRepository repository)
{
    public async Task<Bill> GetBillByToken(long token, CancellationToken cancellationToken)
    {
        var bill = await repository.GetById(token, cancellationToken);

        Guard<BillNotFoundException>.AgainstNull(bill);

        return bill!;
    }
}
