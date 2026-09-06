using FinanceSystem.Domain.Accounts;
using FinanceSystem.Domain.BankAccountDetails;

namespace FinanceSystem.Persistance.Mappings;

public class BankAccountDetailMapping() : EntityBaseMap<BankAccountDetail, long>("BankAccountDetails")
{
    override protected void ConfigureMap(EntityTypeBuilder<BankAccountDetail> builder)
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
            .HasIndex(p => p.AccountId)
            .HasDatabaseName("IX_BankAccountDetails_AccountId");

        builder
            .HasOne(p => p.Account)
            .WithMany()
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_BankAccountDetails_Accounts_AccountId");

        builder
            .Property(p => p.Iban)
            .IsRequired()
            .HasColumnName("Iban")
            .HasMaxLength(50);

        builder
            .HasIndex(p => p.Iban)
            .IsUnique()
            .HasDatabaseName("UX_BankAccountDetails_Iban");

        builder
            .Property(p => p.MaskedPan)
            .HasColumnName("MaskedPan")
            .HasMaxLength(16);

        builder
            .Property(p => p.BankName)
            .HasColumnName("BankName")
            .HasMaxLength(20);
    }
}
