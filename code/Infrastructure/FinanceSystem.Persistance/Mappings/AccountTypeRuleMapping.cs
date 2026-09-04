using FinanceSystem.Domain.AccountTypeRules;

namespace FinanceSystem.Persistance.Mappings;

public class AccountTypeRuleMapping() : EntityBaseMap<AccountTypeRule, int>("AccountTypeRules")
{
    override protected void ConfigureMap(EntityTypeBuilder<AccountTypeRule> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(p => p.Type)
            .HasColumnName("Type");

        builder
            .Property(p => p.Purpose)
            .HasColumnName("Purpose");

        builder
            .Property(p => p.CanBeSource)
            .HasColumnName("CanBeSource");

        builder
            .Property(p => p.CanBeDestination)
            .HasColumnName("CanBeDestination");

        builder
            .HasIndex(p => new { p.Type, p.Purpose })
            .IsUnique()
            .HasDatabaseName("UX_AccountTypeRules_Type_Purpose");
    }
}
