using FinanceSystem.Domain.Contract.Products;

namespace FinanceSystem.Persistance.Mappings
{
    public class ProductEventModelMap : IEntityTypeConfiguration<ProductEventModel>
    {
        public void Configure(EntityTypeBuilder<ProductEventModel> builder)
        {
            builder.ToTable("ProductEvents");
            
            builder.HasKey(a => a.EventId);

            builder.Property(t => t.EventBody).HasColumnName("EventBody");
            builder.Property(t => t.EventType).HasColumnName("EventType");
            builder.Property(t => t.HappenDateTime).HasColumnName("HappenDateTime");
            builder.Property(t => t.EventState).HasColumnName("EventState");
        }
    }
}
