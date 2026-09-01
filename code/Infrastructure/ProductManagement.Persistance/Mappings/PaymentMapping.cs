using ProductManagement.Domain.Accounts;
using ProductManagement.Domain.BankPaymentDetails;
using ProductManagement.Domain.Payments;

namespace ProductManagement.Persistance.Mappings;

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
            .IsRequired()
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
            .Property(p => p.BankPaymentDetailId)
            .HasColumnName("BankPaymentDetailId");

        builder
            .HasIndex(p => p.BankPaymentDetailId)
            .IsUnique()
            .HasDatabaseName("UX_Payments_BankPaymentDetailId");

        builder
            .HasOne<BankPaymentDetail>()
            .WithOne()
            .HasForeignKey<Payment>(p => p.BankPaymentDetailId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_BankPaymentDetails_BankPaymentDetailId");

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(p => p.SourceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_Accounts_SourceAccountId");

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(p => p.DestinationAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Payments_Accounts_DestinationAccountId");

        builder.Ignore(p => p.Publisher);
    }
}
