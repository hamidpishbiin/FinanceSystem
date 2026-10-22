using FinanceSystem.Domain.LedgerEntries;
using FinanceSystem.Domain.LedgerEntries.Enums;
using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.Payments;

namespace FinanceSystem.Persistance.Mappings;

public class LedgerEntryMapping() : EntityBaseMap<LedgerEntry, long>("LedgerEntries")
{
    override protected void ConfigureMap(EntityTypeBuilder<LedgerEntry> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.PaymentId)
            .HasColumnName("PaymentId");

        builder
            .HasIndex(p => p.PaymentId)
            .IncludeProperties(p => p.Amount)
            .HasDatabaseName("IX_LedgerEntries_PaymentId");

        builder
            .HasOne(p => p.Payment)
            .WithMany()
            .HasForeignKey(p => p.PaymentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_LedgerEntries_Payments_PaymentId");

        builder
            .Property(p => p.FinanceAccountId)
            .HasColumnName("FinanceAccountId");

        builder
            .HasIndex(p => p.FinanceAccountId)
            .IncludeProperties(p => new { p.Amount, p.Direction })
            .HasDatabaseName("IX_LedgerEntries_FinanceAccountId");

        builder
            .HasOne(p => p.FinanceAccount)
            .WithMany()
            .HasForeignKey(p => p.FinanceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_LedgerEntries_FinanceAccounts_FinanceAccountId");

        builder
            .Property(p => p.Amount)
            .HasColumnName("Amount")
            .HasPrecision(18, 0);

        builder
            .Property(p => p.Direction)
            .HasColumnName("Direction")
            .HasConversion<byte>();

        builder.Ignore(p => p.SignedAmount);
    }
}
