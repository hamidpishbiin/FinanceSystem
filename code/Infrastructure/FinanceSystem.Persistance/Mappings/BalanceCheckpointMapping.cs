using FinanceSystem.Domain.AccountEntries;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.BalanceCheckpoints;

namespace FinanceSystem.Persistance.Mappings;

public class BalanceCheckpointMapping() : EntityBaseMap<BalanceCheckpoint, long>("BalanceCheckpoints")
{
    override protected void ConfigureMap(EntityTypeBuilder<BalanceCheckpoint> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.FinanceAccountId)
            .IsRequired()
            .HasColumnName("FinanceAccountId");

        builder
            .HasOne(p => p.FinanceAccount)
            .WithMany()
            .HasForeignKey(p => p.FinanceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BalanceCheckpoints_FinanceAccounts_FinanceAccountId");

        builder
            .Property(p => p.UpToEntryId)
            .IsRequired()
            .HasColumnName("UpToEntryId");

        builder
            .HasOne(p => p.UpToEntry)
            .WithMany()
            .HasForeignKey(p => p.UpToEntryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BalanceCheckpoints_AccountEntries_UpToEntryId");

        builder
            .Property(p => p.Balance)
            .HasColumnName("Balance")
            .HasPrecision(18, 0);

        builder
            .HasIndex(p => new { p.FinanceAccountId, p.UpToEntryId })
            .IncludeProperties(p => p.Balance)
            .HasDatabaseName("IX_BalanceCheckpoints_FinanceAccountId_UpToEntryId");
    }
}
