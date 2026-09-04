using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.BankPaymentDetails;
using FinanceSystem.Domain.PaymentServiceProviders;

namespace FinanceSystem.Persistance.Mappings;

public class BankPaymentDetailMapping() : EntityBaseMap<BankPaymentDetail, long>("BankPaymentDetails")
{
    override protected void ConfigureMap(EntityTypeBuilder<BankPaymentDetail> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.PspId)
            .HasColumnName("PspId");

        builder
            .HasOne<PaymentServiceProvider>()
            .WithMany()
            .HasForeignKey(p => p.PspId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BankPaymentDetails_PaymentServiceProviders_PspId");

        builder
            .HasIndex(p => p.PspId)
            .HasDatabaseName("IX_BankPaymentDetails_PspId");

        builder
            .Property(p => p.Status)
            .HasColumnName("Status");

        builder
            .Property(p => p.RequestAmountRial)
            .HasColumnName("RequestAmountRial")
            .HasPrecision(18, 0);

        builder
            .Property(p => p.RedirectedAmountRial)
            .HasColumnName("RedirectedAmountRial")
            .HasPrecision(18, 0);

        builder
            .Property(p => p.TargetAccountId)
            .HasColumnName("TargetAccountId");

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(p => p.TargetAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BankPaymentDetails_Accounts_TargetAccountId");

        builder
            .HasIndex(p => p.TargetAccountId)
            .HasDatabaseName("IX_BankPaymentDetails_TargetAccountId");

        builder
            .Property(p => p.Authority)
            .HasColumnName("Authority")
            .HasMaxLength(20);

        builder
            .Property(p => p.RRN)
            .HasColumnName("RRN")
            .HasMaxLength(20);

        builder
            .HasIndex(p => p.RRN)
            .IsUnique()
            .HasDatabaseName("UX_BankPaymentDetails_RRN");

        builder
            .Property(p => p.RefNum)
            .HasColumnName("RefNum")
            .HasMaxLength(20);

        builder
            .HasIndex(p => p.RefNum)
            .IsUnique()
            .HasDatabaseName("UX_BankPaymentDetails_RefNum");

        builder
            .Property(p => p.TraceNumber)
            .HasColumnName("TraceNumber")
            .HasMaxLength(20);

        builder
            .Property(p => p.MaskedPan)
            .HasMaxLength(20)
            .HasColumnName("MaskedPan");

        builder
            .Property(p => p.ResultCode)
            .HasColumnName("ResultCode");

        builder
            .Property(p => p.RawCallback)
            .HasColumnName("RawCallback");

        builder
            .Property(p => p.VerifiedAtUtc)
            .HasColumnName("VerifiedAtUtc");

        builder.Ignore(p => p.Publisher);
    }
}
