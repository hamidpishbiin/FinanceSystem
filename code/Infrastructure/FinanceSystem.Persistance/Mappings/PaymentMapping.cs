using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.PspPaymentDetails;
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
            .Property(p => p.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("IdempotencyKey");

        builder
            .HasIndex(p => p.IdempotencyKey)
            .IsUnique()
            .HasDatabaseName("UX_Payments_IdempotencyKey");

        builder
            .Property(p => p.Purpose)
            .IsRequired()
            .HasColumnName("Purpose");

        builder
            .Property(p => p.Channel)
            .IsRequired()
            .HasColumnName("Channel");

        builder
            .Property(p => p.AmountRial)
            .HasColumnName("AmountRial");

        builder
            .Property(p => p.SourceAccountId)
            .HasColumnName("SourceAccountId");

        builder
            .Property(p => p.DestinationAccountId)
            .HasColumnName("DestinationAccountId");

        builder
            .Property(p => p.OriginServiceId)
            .HasMaxLength(50)
            .IsRequired()
            .HasColumnName("OriginServiceId");

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
            .Property(p => p.PspPaymentDetailId)
            .HasColumnName("PspPaymentDetailId");

        builder
            .HasIndex(p => p.PspPaymentDetailId)
            .IsUnique()
            .HasDatabaseName("UX_Payments_PspPaymentDetailId");

        builder
            .HasOne(p => p.PspPaymentDetail)
            .WithOne()
            .HasForeignKey<Payment>(p => p.PspPaymentDetailId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_PspPaymentDetails_PspPaymentDetailId");

        builder
            .HasOne(p => p.SourceAccount)
            .WithMany()
            .HasForeignKey(p => p.SourceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_Accounts_SourceAccountId");

        builder
            .HasOne(p => p.DestinationAccount)
            .WithMany()
            .HasForeignKey(p => p.DestinationAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_Accounts_DestinationAccountId");

        builder.Ignore(p => p.Publisher);
    }
}
