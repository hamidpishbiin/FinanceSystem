using FinanceSystem.Domain.PspExchangeLogs;

namespace FinanceSystem.Persistance.Mappings;

public class PspExchangeLogMapping() : EntityBaseMap<PspExchangeLog, long>("PspExchangeLogs")
{
    override protected void ConfigureMap(EntityTypeBuilder<PspExchangeLog> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Step)
            .IsRequired()
            .HasColumnName("Step")
            .HasConversion<byte>();

        builder
            .Property(p => p.ReferenceId)
            .IsRequired()
            .HasColumnName("ReferenceId");

        builder
            .HasIndex(p => p.ReferenceId)
            .HasDatabaseName("IX_PspExchangeLogs_ReferenceId");

        builder
            .Property(p => p.RequestBody)
            .HasColumnName("RequestBody");

        builder
            .Property(p => p.ResponseBody)
            .HasColumnName("ResponseBody");

        builder
            .Property(p => p.HttpStatus)
            .HasColumnName("HttpStatus");

        builder
            .Property(p => p.DurationMs)
            .HasColumnName("DurationMs");
    }
}
