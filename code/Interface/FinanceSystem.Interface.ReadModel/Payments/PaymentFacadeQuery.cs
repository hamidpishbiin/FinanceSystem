using FinanceSystem.Domain.Bills.Exceptions;
using FinanceSystem.Interface.Contracts.Payments.DTOs;
using FinanceSystem.Interface.Contracts.Payments.Models;
using FinanceSystem.Interface.Contracts.Payments.Services;
using FinanceSystem.Interface.ReadModel.Bills;
using FinanceSystem.Interface.ReadModel.FinanceAccounts;
using FinanceSystem.Interface.ReadModel.PaymentServiceProviders;
using Shared.Core.Exceptions;
using Shared.Presentation;

namespace FinanceSystem.Interface.ReadModel.Payments;

internal class PaymentFacadeQuery(
    IBillQueryRepository billQueryRepository,
    IPspQueryRepository pspQueryRepository,
    IFinanceAccountQueryRepository financeAccountQueryRepository) : IPaymentFacadeQuery
{
    public async Task<JsonResponse<List<PaymentOptionDto>>> GetPaymentOptionsAsync(PaymentOptionsRequestModel model, CancellationToken cancellationToken)
    {
        var result = new List<PaymentOptionDto>();

        var bill = await billQueryRepository.GetAsync(long.Parse(model.Token), model.UserId!.Value, cancellationToken);

        if (bill is null) return JsonResponse<List<PaymentOptionDto>>.Success(result);

        var activePsps = await pspQueryRepository.GetActivePspsAsync(cancellationToken);

        result.AddRange(activePsps.ToPaymentOptionDtoList());

        var activeFinanceAccounts = await financeAccountQueryRepository.GetActiveAccountsAsync(model.UserId!.Value, bill!.Amount, cancellationToken);

        result.AddRange(activeFinanceAccounts.ToPaymentOptionDtoList());

        return JsonResponse<List<PaymentOptionDto>>.Success(result);
    }
}
