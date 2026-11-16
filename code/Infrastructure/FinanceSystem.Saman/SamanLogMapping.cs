using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSystem.Saman;

internal static class SamanLogMapping
{
    public static void Configure<TLog>(EntityTypeBuilder<TLog> builder) where TLog : SamanLogModel
    {
        builder.HasKey(l => l.Id);

        builder
            .Property(l => l.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(l => l.CreatedAt)
            .HasColumnName("CreatedAt");

        builder
            .Property(l => l.ErrorType)
            .HasColumnName("ErrorType");

        builder
            .Property(l => l.ErrorMessage)
            .HasColumnName("ErrorMessage");
    }

    public static void ConfigureExchange<TLog>(EntityTypeBuilder<TLog> builder) where TLog : SamanExchangeLogModel
    {
        Configure(builder);

        builder
            .Property(l => l.HttpStatusCode)
            .HasColumnName("HttpStatusCode");
    }
}
