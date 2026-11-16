using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSystem.Saman;

internal static class SamanVerificationResponseMapping
{
    public static void Configure<TLog>(OwnedNavigationBuilder<TLog, SamanPaymentVerificationResponse> response) where TLog : class
    {
        response.Ignore(r => r.PspTransactionType);

        response
            .Property(r => r.Success)
            .HasColumnName("ResponseSuccess");

        response
            .Property(r => r.ResultCode)
            .HasColumnName("ResponseResultCode");

        response
            .Property(r => r.ResultDescription)
            .HasColumnName("ResponseResultDescription");

        response.OwnsOne(r => r.TransactionDetail, detail =>
        {
            detail
                .Property(d => d.RRN)
                .HasColumnName("ResponseRRN");

            detail
                .Property(d => d.RefNum)
                .HasColumnName("ResponseRefNum");

            detail
                .Property(d => d.MaskedPan)
                .HasColumnName("ResponseMaskedPan");

            detail
                .Property(d => d.HashedPan)
                .HasColumnName("ResponseHashedPan");

            detail
                .Property(d => d.TerminalNumber)
                .HasColumnName("ResponseTerminalNumber");

            detail
                .Property(d => d.OrginalAmount)
                .HasColumnName("ResponseOrginalAmount");

            detail
                .Property(d => d.AffectiveAmount)
                .HasColumnName("ResponseAffectiveAmount");

            detail
                .Property(d => d.StraceDate)
                .HasColumnName("ResponseStraceDate");

            detail
                .Property(d => d.StraceNo)
                .HasColumnName("ResponseStraceNo");
        });
    }
}
