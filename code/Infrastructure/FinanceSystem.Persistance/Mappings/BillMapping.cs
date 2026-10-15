using FinanceSystem.Domain.Bills;

namespace FinanceSystem.Persistance.Mappings;

public class BillMapping() : EntityBaseMap<Bill, long>("Bills")
{
    override protected void ConfigureMap(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);

        builder
            .Property(b => b.ServiceId)
            .HasColumnName("ServiceId");

        builder
            .Property(b => b.ReferenceId)
            .HasColumnName("ReferenceId");

        builder
            .Property(b => b.Amount)
            .HasColumnName("Amount");

        builder.Ignore(b => b.Publisher);
    }
}
