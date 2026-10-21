using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Interface.Contracts.Payments.DTOs;

namespace FinanceSystem.Interface.ReadModel.FinanceAccounts;

internal static class FinanceAccountMappers
{
    public static List<PaymentOptionDto> ToPaymentOptionDtoList(this IEnumerable<FinanceAccount> models)
    {
        return [.. models.Select(ToPaymentOptionDto) ];
    }

    public static PaymentOptionDto ToPaymentOptionDto(this FinanceAccount model)
    {
        return new PaymentOptionDto()
        {
            Name = model.Type.ToString(),
            Channel = (int)model.Type,
        };
    }
}
