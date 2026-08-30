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
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("SourceAccountId");

        builder
            .Property(p => p.DestinationAccountId)
            .IsRequired()
            .HasMaxLength(50)
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
            .HasFilter("\"BankPaymentDetailId\" IS NOT NULL")
            .HasDatabaseName("UX_Payments_BankPaymentDetailId");

        builder.Ignore(p => p.Publisher);
    }
}
