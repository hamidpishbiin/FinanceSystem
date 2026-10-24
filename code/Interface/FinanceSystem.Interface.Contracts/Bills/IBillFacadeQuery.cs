using FinanceSystem.Interface.Contracts.Payments.DTOs;
using FinanceSystem.Interface.Contracts.Payments.Models;
using Shared.Core;
using Shared.Presentation;

namespace FinanceSystem.Interface.Contracts.Bills;

public interface IBillFacadeQuery : IFacadeService
{
    Task<JsonResponse<List<PaymentOptionDto>>> GetPaymentOptionsAsync(long billId, long userId, CancellationToken cancellationToken);
}
