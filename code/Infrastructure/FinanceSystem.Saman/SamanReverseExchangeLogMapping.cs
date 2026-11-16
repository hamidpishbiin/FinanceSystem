using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSystem.Saman;

internal class SamanReverseExchangeLogMapping : IEntityTypeConfiguration<SamanReverseExchangeLogModel>
{
    public void Configure(EntityTypeBuilder<SamanReverseExchangeLogModel> builder)
    {
        builder.ToTable("SamanReverseExchangeLogs");

        SamanLogMapping.ConfigureExchange(builder);

        builder
            .Property(l => l.RequestReferenceNumber)
            .HasColumnName("RequestReferenceNumber");

        builder
            .Property(l => l.RawResponseBody)
            .HasColumnName("RawResponseBody");

        builder.OwnsOne(l => l.Response, SamanVerificationResponseMapping.Configure);

        builder
            .HasIndex(l => l.RequestReferenceNumber)
            .HasDatabaseName("IX_SamanReverseExchangeLogs_RequestReferenceNumber");
    }
}
