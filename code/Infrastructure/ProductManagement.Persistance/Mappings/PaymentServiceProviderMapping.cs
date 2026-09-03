using ProductManagement.Domain.PaymentServiceProviders;

namespace ProductManagement.Persistance.Mappings;

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
            .Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(20);

        builder
            .Property(p => p.IsActive)
            .HasColumnName("IsActive");

        builder
            .Property(p => p.Priority)
            .HasColumnName("Priority");

        builder
            .Property(p => p.MerchantId)
            .IsRequired()
            .HasColumnName("MerchantId")
            .HasMaxLength(30);

        builder
            .Property(p => p.TerminalId)
            .IsRequired()
            .HasColumnName("TerminalId")
            .HasMaxLength(20);

        builder
            .Property(p => p.CredentialsRef)
            .IsRequired()
            .HasColumnName("CredentialsRef")
            .HasMaxLength(20);

        builder
            .Property(p => p.BaseUrl)
            .IsRequired()
            .HasColumnName("BaseUrl");

        builder
            .Property(p => p.CallbackUrl)
            .IsRequired()
            .HasColumnName("CallbackUrl");
    }
}
