using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Payments.Services;

public interface IPaymentFacadeService : IFacadeService
{
    [IgnorePermission]
    Task<JsonResponse<string>> ProcessPaymentCallbackAsync(string psp, Dictionary<string, string> fields, CancellationToken cancellationToken);
}
