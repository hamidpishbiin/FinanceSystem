using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Persistance.Mappings;

public class PaymentMapping() : EntityBaseMap<Payment, long>("Payments")
{
    override protected void ConfigureMap(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Purpose)
            .IsRequired()
            .HasColumnName("Purpose");

        builder
            .Property(p => p.Channel)
            .IsRequired()
            .HasColumnName("Channel");

        builder
            .Property(p => p.Amount)
            .HasColumnName("Amount");

        builder
            .Property(p => p.SourceFinanceAccountId)
            .HasColumnName("SourceFinanceAccountId");

        builder
            .Property(p => p.DestinationFinanceAccountId)
            .HasColumnName("DestinationFinanceAccountId");

        builder
            .Property(p => p.ExternalServiceId)
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnName("ExternalServiceId");

        builder
            .Property(p => p.ExternalReferenceId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("ExternalReferenceId");

        builder
            .Property(p => p.ExternalTag)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("ExternalTag");

        builder
            .Property(p => p.State)
            .IsRequired()
            .HasColumnName("State");

        builder
            .Property(p => p.PspCode)
            .HasColumnName("PspCode");

        builder
            .HasOne(p => p.Psp)
            .WithMany()
            .HasForeignKey(p => p.PspCode)
            .HasPrincipalKey(psp => psp.Code)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_PaymentServiceProviders_PspCode");

        builder
            .HasIndex(p => p.PspCode)
            .HasDatabaseName("IX_Payments_PspCode");

        builder
            .Property(p => p.RedirectedAmount)
            .HasColumnName("RedirectedAmount")
            .HasPrecision(18, 0);

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
            .HasDatabaseName("UX_Payments_RRN");

        builder
            .Property(p => p.ReferenceNumber)
            .HasColumnName("ReferenceNumber")
            .HasMaxLength(20);

        builder
            .HasIndex(p => p.ReferenceNumber)
            .IsUnique()
            .HasDatabaseName("UX_Payments_ReferenceNumber");

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
            .Property(p => p.Status)
            .HasColumnName("RawStatus")
            .HasMaxLength(20);

        builder
            .Property(p => p.StatusCode)
            .HasColumnName("RawStatusCode");

        builder
            .Property(p => p.RawDescription)
            .HasColumnName("RawDescription");

        builder
            .Property(p => p.SucceededAtUtc)
            .HasColumnName("SucceededAtUtc");

        builder
            .HasOne(p => p.SourceFinanceAccount)
            .WithMany()
            .HasForeignKey(p => p.SourceFinanceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_FinanceAccounts_SourceFinanceAccountId");

        builder
            .HasOne(p => p.DestinationFinanceAccount)
            .WithMany()
            .HasForeignKey(p => p.DestinationFinanceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_FinanceAccounts_DestinationFinanceAccountId");

        builder.Ignore(p => p.Publisher);
    }
}
