using FinanceSystem.Core;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.TopUps.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.TopUps.Services;

public interface ITopUpFacadeService : IFacadeService
{
    [HasPermission(Permissions.CreatePayment, (int)UserRoleEnum.Owner)]
    Task<JsonResponse<string>> Create(CreatePaymentModel model);
    [HasPermission(Permissions.CreatePayment, (int)UserRoleEnum.Owner)]
    Task<JsonResponse<string>> TopUpAsync(TopUpModel model, CancellationToken cancellationToken = default);
    [IgnorePermission]
    Task<JsonResponse<string>> CompleteTopUpProcess(string psp, IReadOnlyDictionary<string, string> fields, CancellationToken cancellationToken = default);
}
