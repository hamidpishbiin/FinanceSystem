using ProductManagement.Domain.AccountEntries;
using ProductManagement.Domain.Accounts;
using ProductManagement.Domain.BalanceCheckpoints;

namespace ProductManagement.Persistance.Mappings;

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
            .Property(p => p.AccountId)
            .HasColumnName("AccountId");

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BalanceCheckpoints_Accounts_AccountId");

        builder
            .Property(p => p.UpToEntryId)
            .HasColumnName("UpToEntryId");

        builder
            .HasOne<AccountEntry>()
            .WithMany()
            .HasForeignKey(p => p.UpToEntryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BalanceCheckpoints_AccountEntries_UpToEntryId");

        builder
            .Property(p => p.BalanceRial)
            .HasColumnName("BalanceRial")
            .HasPrecision(18, 0);

        builder
            .HasIndex(p => new { p.AccountId, p.UpToEntryId })
            .IncludeProperties(p => p.BalanceRial)
            .HasDatabaseName("IX_BalanceCheckpoints_AccountId_UpToEntryId");
    }
}
