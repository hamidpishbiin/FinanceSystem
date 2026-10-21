using FinanceSystem.Interface.Contracts.Payments.DTOs;
using FinanceSystem.Interface.Contracts.Payments.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Payments.Services;

public interface IPaymentFacadeQuery : IFacadeService
{
    [IgnorePermission]
    Task<JsonResponse<List<PaymentOptionDto>>> GetPaymentOptionsAsync(PaymentOptionsRequestModel model, CancellationToken cancellationToken);
}
