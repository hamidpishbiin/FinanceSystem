using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceSystem.Saman;

internal class SamanCallbackLogMapping : IEntityTypeConfiguration<SamanCallbackLogModel>
{
    public void Configure(EntityTypeBuilder<SamanCallbackLogModel> builder)
    {
        builder.ToTable("SamanCallbackLogs");

        SamanLogMapping.Configure(builder);

        builder
            .Property(l => l.RawFields)
            .HasColumnName("RawFields");

        builder
            .Property(l => l.Status)
            .HasColumnName("Status");

        builder
            .Property(l => l.ResNum)
            .HasColumnName("ResNum");

        builder
            .Property(l => l.Token)
            .HasColumnName("Token");

        builder
            .Property(l => l.RefNum)
            .HasColumnName("RefNum");

        builder
            .Property(l => l.Amount)
            .HasColumnName("Amount");

        builder
            .Property(l => l.SecurePan)
            .HasColumnName("SecurePan");

        builder
            .Property(l => l.RRN)
            .HasColumnName("RRN");

        builder
            .Property(l => l.TraceNo)
            .HasColumnName("TraceNo");

        builder
            .HasIndex(l => l.ResNum)
            .HasDatabaseName("IX_SamanCallbackLogs_ResNum");
    }
}
