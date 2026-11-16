using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSystem.Saman;

internal class SamanVerifyExchangeLogMapping : IEntityTypeConfiguration<SamanVerifyExchangeLogModel>
{
    public void Configure(EntityTypeBuilder<SamanVerifyExchangeLogModel> builder)
    {
        builder.ToTable("SamanVerifyExchangeLogs");

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
            .HasDatabaseName("IX_SamanVerifyExchangeLogs_RequestReferenceNumber");
    }
}
