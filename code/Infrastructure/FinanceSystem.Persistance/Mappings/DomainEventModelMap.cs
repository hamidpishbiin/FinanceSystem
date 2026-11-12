using FinanceSystem.Domain.Contract;

namespace FinanceSystem.Persistance.Mappings;

public class DomainEventModelMap : IEntityTypeConfiguration<DomainEventModel>
{
    public void Configure(EntityTypeBuilder<DomainEventModel> builder)
    {
        builder.ToTable("DomainEvents");

        builder.HasKey(e => e.EventId);

        builder
            .Property(e => e.EventId)
            .HasColumnName("EventId")
            .ValueGeneratedNever();

        builder
            .Property(e => e.HappenDateTime)
            .IsRequired()
            .HasColumnName("HappenDateTime");

        builder
            .Property(e => e.EventState)
            .IsRequired()
            .HasColumnName("EventState");

        builder
            .Property(e => e.EntityName)
            .IsRequired()
            .HasColumnName("EntityName");

        builder
            .Property(e => e.EventType)
            .IsRequired()
            .HasColumnName("EventType");

        builder
            .Property(e => e.EventBody)
            .IsRequired()
            .HasColumnName("EventBody");

        builder
            .HasIndex(e => e.HappenDateTime)
            .HasDatabaseName("IX_DomainEvents_HappenDateTime");
    }
}
