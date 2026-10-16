using FinanceSystem.Interface.Contracts.Bills.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Bills.Services;

public interface IBillFacadeService : IFacadeService
{
    Task<JsonResponse<long>> CreateBill(CreateBillModel model, string userId, CancellationToken cancellationToken);
}
