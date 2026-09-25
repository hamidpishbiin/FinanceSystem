using FinanceSystem.Domain.PaymentServiceProviders;

namespace FinanceSystem.Persistance.Mappings;

public class PaymentServiceProviderMapping() : EntityBaseMap<PaymentServiceProvider, Guid>("PaymentServiceProviders")
{
    override protected void ConfigureMap(EntityTypeBuilder<PaymentServiceProvider> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id");

        builder
            .Property(p => p.Code)
            .HasColumnName("Code");

        builder
            .HasIndex(p => p.Code)
            .IsUnique()
            .HasDatabaseName("IX_PaymentServiceProviders_Code");

        builder
            .Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(50);

        builder
            .Property(p => p.IsActive)
            .HasColumnName("IsActive");

        builder
            .Property(p => p.Priority)
            .HasColumnName("Priority");
    }
}
