using FinanceSystem.Domain.Accounts;

namespace FinanceSystem.Persistance.Mappings;

public class AccountMapping() : EntityBaseMap<Account, long>("Accounts")
{
    override protected void ConfigureMap(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.OwnerId)
            .HasColumnName("OwnerId");

        // builder
        //     .HasOne<User>()
        //     .WithMany()
        //     .HasForeignKey(p => p.OwnerId)
        //     .OnDelete(DeleteBehavior.Restrict)
        //     .HasConstraintName("FK_Accounts_Users_OwnerId");

        builder
            .Property(p => p.AllowNegativeBalance)
            .HasColumnName("AllowNegativeBalance");

        builder
            .Property(p => p.BalanceCalculatedAt)
            .HasColumnName("BalanceCalculatedAt");

        builder
            .Property(p => p.CachedBalanceRial)
            .HasColumnName("CachedBalanceRial")
            .HasPrecision(18, 0);

        builder
            .Property(p => p.Status)
            .HasColumnName("Status");

        builder
            .Property(p => p.Type)
            .HasColumnName("Type");

        builder
            .Property<uint>("xmin")
            .HasColumnType("xid")
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();
    }
}
