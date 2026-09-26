using FinanceSystem.Domain.FinanceAccounts;
using FinanceSystem.Domain.Users;

namespace FinanceSystem.Persistance.Mappings;

public class FinanceAccountMapping() : EntityBaseMap<FinanceAccount, long>("FinanceAccounts")
{
    override protected void ConfigureMap(EntityTypeBuilder<FinanceAccount> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.UserId)
            .HasColumnName("UserId");

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_FinanceAccounts_Users_UserId");

        builder
            .Property(p => p.AllowNegativeBalance)
            .HasColumnName("AllowNegativeBalance");

        builder
            .Property(p => p.BalanceCalculatedAt)
            .HasColumnName("BalanceCalculatedAt");

        builder
            .Property(p => p.CachedBalance)
            .HasColumnName("CachedBalance")
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
