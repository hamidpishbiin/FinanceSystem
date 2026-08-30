using ProductManagement.Core;
using ProductManagement.Interface.Contracts.Payments.Models;
using Shared.Core;
using Shared.Presentation;

namespace ProductManagement.Interface.Contracts.Payments.Services;

public interface IPaymentFacadeService : IFacadeService
{
    [HasPermission(Permissions.CreatePayment, (int)UserRoleEnum.Owner)]
    Task<JsonResponse<string>> Create(CreatePaymentModel model);
}
