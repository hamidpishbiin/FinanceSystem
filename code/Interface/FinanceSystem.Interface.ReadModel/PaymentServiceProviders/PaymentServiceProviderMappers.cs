using FinanceSystem.Domain.Payments.Enums;
using FinanceSystem.Domain.PaymentServiceProviders;
using FinanceSystem.Interface.Contracts.Payments.DTOs;

namespace FinanceSystem.Interface.ReadModel.PaymentServiceProviders;

internal static class PaymentServiceProviderMappers
{
    public static List<PaymentOptionDto> ToPaymentOptionDtoList(this List<PaymentServiceProvider> models)
    {
        return [..models.Select(ToPaymentOptionDto)];
    }

    public static PaymentOptionDto ToPaymentOptionDto(this PaymentServiceProvider model)
    {
        return new PaymentOptionDto()
        {
            Name = model.Name,
            Channel = (int)PaymentChannel.Psp,
            PspCode = (int)model.Code
        };
    }
}
