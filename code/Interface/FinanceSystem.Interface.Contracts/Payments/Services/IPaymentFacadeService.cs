using FinanceSystem.Core;
using FinanceSystem.Interface.Contracts.Payments.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Payments.Services;

public interface IPaymentFacadeService : IFacadeService
{
    [HasPermission(Permissions.CreatePayment, (int)UserRoleEnum.Owner)]
    Task<JsonResponse<string>> RequestPaymentTokenAsync(PspPaymentModel model, CancellationToken cancellationToken = default);
    [IgnorePermission]
    Task<JsonResponse<string>> CompletePaymentProcessAsync(string psp, Dictionary<string, string> fields, CancellationToken cancellationToken);
}
