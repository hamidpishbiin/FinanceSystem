using FinanceSystem.Core;
using FinanceSystem.Interface.Contracts.Payments.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Payments.Services;

public interface IPaymentFacadeService : IFacadeService
{
    [HasPermission(Permissions.CreatePayment, (int)UserRoleEnum.Owner)]
    Task<JsonResponse<string>> Create(CreatePaymentModel model);
}
