using FinanceSystem.Domain.Bills;

namespace FinanceSystem.Persistance.Mappings;

public class BillMapping() : EntityBaseMap<Bill, long>("Bills")
{
    override protected void ConfigureMap(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.ServiceId)
            .HasColumnName("ServiceId");

        builder
            .Property(b => b.ReferenceId)
            .HasColumnName("ReferenceId");

        builder
            .Property(b => b.Amount)
            .HasColumnName("Amount");

        builder
            .Property(b => b.FinanceAccountId)
            .HasColumnName("FinanceAccountId");

        builder
            .HasOne(b => b.FinanceAccount)
            .WithMany()
            .HasForeignKey(b => b.FinanceAccountId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Bills_FinanceAccounts_FinanceAccountId");

        builder.Ignore(b => b.Publisher);
    }
}
