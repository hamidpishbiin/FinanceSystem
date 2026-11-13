using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSystem.Saman;

internal class SamanTokenExchangeLogMapping : IEntityTypeConfiguration<SamanTokenExchangeLogModel>
{
    public void Configure(EntityTypeBuilder<SamanTokenExchangeLogModel> builder)
    {
        builder.ToTable("SamanTokenExchangeLogs");

        builder.HasKey(l => l.Id);

        builder
            .Property(l => l.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(l => l.RequestAmount)
            .HasColumnName("RequestAmount");

        builder
            .Property(l => l.RequestReservationNumber)
            .HasColumnName("RequestReservationNumber");

        builder
            .Property(l => l.RawResponseBody)
            .HasColumnName("RawResponseBody");

        builder.OwnsOne(l => l.Response, response =>
        {
            response
                .Property(r => r.Status)
                .HasColumnName("ResponseStatus");

            response
                .Property(r => r.Token)
                .HasColumnName("ResponseToken");

            response
                .Property(r => r.ErrorCode)
                .HasColumnName("ResponseErrorCode");

            response
                .Property(r => r.ErrorDesc)
                .HasColumnName("ResponseErrorDesc");
        });

        builder
            .HasIndex(l => l.RequestReservationNumber)
            .HasDatabaseName("IX_SamanTokenExchangeLogs_RequestReservationNumber");
    }
}
