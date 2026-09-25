using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.RequestsToPay;
using FinanceSystem.Domain.PaymentServiceProviders;

namespace FinanceSystem.Persistance.Mappings;

public class RequestToPayMapping() : EntityBaseMap<RequestToPay, long>("RequestsToPay")
{
    override protected void ConfigureMap(EntityTypeBuilder<RequestToPay> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.PspCode)
            .HasColumnName("PspCode");

        builder
            .HasOne(p => p.Psp)
            .WithMany()
            .HasForeignKey(p => p.PspCode)
            .HasPrincipalKey(psp => psp.Code)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_RequestsToPay_PaymentServiceProviders_PspCode");

        builder
            .HasIndex(p => p.PspCode)
            .HasDatabaseName("IX_RequestsToPay_PspCode");

        builder
            .Property(p => p.Status)
            .HasColumnName("Status");

        builder
            .Property(p => p.RequestAmount)
            .HasColumnName("RequestAmount")
            .HasPrecision(18, 0);

        builder
            .Property(p => p.RedirectedAmount)
            .HasColumnName("RedirectedAmount")
            .HasPrecision(18, 0);

        builder
            .Property(p => p.TargetAccountId)
            .HasColumnName("TargetAccountId");

        builder
            .HasOne(p => p.TargetAccount)
            .WithMany()
            .HasForeignKey(p => p.TargetAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_RequestsToPay_Accounts_TargetAccountId");

        builder
            .HasIndex(p => p.TargetAccountId)
            .HasDatabaseName("IX_RequestsToPay_TargetAccountId");

        builder
            .Property(p => p.ReferenceNumber)
            .HasColumnName("ReferenceNumber")
            .HasDefaultValueSql("nextval('\"RequestsToPay_ReferenceNumber_seq\"')")
            .ValueGeneratedOnAdd();

        builder
            .HasIndex(p => p.ReferenceNumber)
            .IsUnique()
            .HasDatabaseName("UX_RequestsToPay_ReferenceNumber");

        builder
            .Property(p => p.Token)
            .HasColumnName("Token")
            .HasMaxLength(20);

        builder
            .Property(p => p.RRN)
            .HasColumnName("RRN")
            .HasMaxLength(20);

        builder
            .HasIndex(p => p.RRN)
            .IsUnique()
            .HasDatabaseName("UX_RequestsToPay_RRN");

        builder
            .Property(p => p.RefNum)
            .HasColumnName("RefNum")
            .HasMaxLength(20);

        builder
            .HasIndex(p => p.RefNum)
            .IsUnique()
            .HasDatabaseName("UX_RequestsToPay_RefNum");

        builder
            .Property(p => p.TraceNumber)
            .HasColumnName("TraceNumber")
            .HasMaxLength(20);

        builder
            .Property(p => p.MaskedPan)
            .HasMaxLength(20)
            .HasColumnName("MaskedPan");

        builder
            .Property(p => p.FailureReason)
            .HasColumnName("FailureReason");

        builder
            .Property(p => p.RawStatus)
            .HasColumnName("RawStatus")
            .HasMaxLength(20);

        builder
            .Property(p => p.RawErrorCode)
            .HasColumnName("RawErrorCode")
            .HasMaxLength(20);

        builder
            .Property(p => p.RawDescription)
            .HasColumnName("RawDescription");

        builder
            .Property(p => p.VerifiedAtUtc)
            .HasColumnName("VerifiedAtUtc");

        builder.Ignore(p => p.Publisher);
    }
}
