using ProductManagement.Domain.AccountEntries;
using ProductManagement.Domain.Accounts;
using ProductManagement.Domain.Payments;

namespace ProductManagement.Persistance.Mappings;

public class AccountEntryMapping() : EntityBaseMap<AccountEntry, long>("AccountEntries")
{
    override protected void ConfigureMap(EntityTypeBuilder<AccountEntry> builder)
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
            .IncludeProperties(p => p.AmountRial)
            .HasDatabaseName("IX_AccountEntries_PaymentId");

        builder
            .HasOne<Payment>()
            .WithMany()
            .HasForeignKey(p => p.PaymentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_AccountEntries_Payments_PaymentId");

        builder
            .Property(p => p.AccountId)
            .HasColumnName("AccountId");

        builder
            .HasIndex(p => p.AccountId)
            .IncludeProperties(p => p.AmountRial)
            .HasDatabaseName("IX_AccountEntries_AccountId");

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_AccountEntries_Accounts_AccountId");

        builder
            .Property(p => p.AmountRial)
            .HasColumnName("AmountRial")
            .HasPrecision(18, 0);
    }
}
