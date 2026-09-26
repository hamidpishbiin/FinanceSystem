using FinanceSystem.Domain.Users;

namespace FinanceSystem.Persistance.Mappings;

public class UserMapping() : EntityBaseMap<User, Guid>("Users")
{
    override protected void ConfigureMap(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever();

        builder
            .Property(p => p.FirstName)
            .HasColumnName("FirstName");

        builder
            .Property(p => p.LastName)
            .HasColumnName("LastName");

        builder
            .Property(p => p.PhoneNumber)
            .HasColumnName("PhoneNumber");

        builder
            .Property(p => p.NationalCode)
            .HasColumnName("NationalCode");

        builder
            .HasIndex(p => p.NationalCode)
            .IsUnique()
            .HasDatabaseName("IX_Users_NationalCode");
    }
}
