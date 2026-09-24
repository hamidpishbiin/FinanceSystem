using FinanceSystem.Domain.PaymentServiceProviders.Enums;

namespace FinanceSystem.Application.Payments.Gateways;

public class PspOptions
{
    public const string SectionName = "PspOptions";

    public PspSettings Saman { get; set; } = default!;
    public PspSettings BehPardakht { get; set; } = default!;

    public PspSettings For(PspCode pspCode)
    {
        return pspCode switch
        {
            PspCode.Saman => Saman,
            PspCode.BehPardakht => BehPardakht,
            _ => throw new InvalidOperationException($"No {SectionName} entry configured for PSP '{pspCode}'.")
        };
    }
}
