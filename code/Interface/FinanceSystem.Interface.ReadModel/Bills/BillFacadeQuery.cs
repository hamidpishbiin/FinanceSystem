using FinanceSystem.Domain.FinanceAccounts.Enums;
using FinanceSystem.Interface.Contracts.Bills;
using FinanceSystem.Interface.Contracts.Payments.DTOs;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.ReadModel.FinanceAccounts;
using FinanceSystem.Interface.ReadModel.PaymentServiceProviders;
using Shared.Presentation;

namespace FinanceSystem.Interface.ReadModel.Bills;

public class BillFacadeQuery(
    IBillQueryRepository billQueryRepository,
    IPspQueryRepository pspQueryRepository,
    IFinanceAccountQueryRepository financeAccountQueryRepository) : IBillFacadeQuery
{
    public async Task<JsonResponse<List<PaymentOptionDto>>> GetPaymentOptionsAsync(long billId, long userId, CancellationToken cancellationToken)
    {
        var result = new List<PaymentOptionDto>();

        var bill = await billQueryRepository.GetAsync(billId, userId, cancellationToken);

        if (bill is null || bill.UserId != userId)
        {
            return JsonResponse<List<PaymentOptionDto>>.Failure(result);
        }

        var activePsps = await pspQueryRepository.GetActivePspsAsync(cancellationToken);

        result.AddRange(activePsps.ToPaymentOptionDtoList());

        var userAccounts = await financeAccountQueryRepository.GetUserAccountsAsync(userId, cancellationToken);

        if (userAccounts.Count == 0) return JsonResponse<List<PaymentOptionDto>>.Failure(result);

        var activeAccounts = userAccounts.Where(ua => ua.State == FinanceAccountState.Active).ToList();

        if (activeAccounts.Count == 0) return JsonResponse<List<PaymentOptionDto>>.Failure(result);

        // Validate wallet balance then return the finance account

        result.AddRange(userAccounts.ToPaymentOptionDtoList());

        return JsonResponse<List<PaymentOptionDto>>.Success(result);
    }
}
