using FinanceSystem.Domain.Contract.Payments;

namespace FinanceSystem.Persistance.Mappings;

public class PaymentEventModelMap : IEntityTypeConfiguration<PaymentEventModel>
{
    public void Configure(EntityTypeBuilder<PaymentEventModel> builder)
    {
        builder.ToTable("PaymentEvents");

        builder.HasKey(a => a.EventId);

        builder.Property(t => t.EventBody).HasColumnName("EventBody");
        builder.Property(t => t.EventType).HasColumnName("EventType");
        builder.Property(t => t.HappenDateTime).HasColumnName("HappenDateTime");
        builder.Property(t => t.EventState).HasColumnName("EventState");
    }
}
