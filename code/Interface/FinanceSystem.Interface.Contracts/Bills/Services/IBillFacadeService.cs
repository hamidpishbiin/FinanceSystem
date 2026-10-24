using FinanceSystem.Interface.Contracts.Bills.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Bills.Services;

public interface IBillFacadeService : IFacadeService
{
    [IgnorePermission]
    Task<JsonResponse<string>> PayWithPspAsync(PspPaymentRequest request, long userId, CancellationToken cancellationToken = default);
    [IgnorePermission]
    Task<JsonResponse<long>> CreateBill(CreateBillRequest request, CancellationToken cancellationToken);
    [IgnorePermission]
    Task<JsonResponse<string>> PayWithWalletAsync(long billId, long userId, long financeAccountId, CancellationToken cancellationToken);
}
