global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Shared.EF;

using FinanceSystem.Domain.Products;

namespace FinanceSystem.Persistance.Mappings
{
    public class ProductMapping : EntityBaseMap<Product, Guid>
    {
        public ProductMapping() : base("Products")
        {
        }

        protected override void ConfigureMap(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("Id");
            builder.Property(a => a.Name).HasColumnName("Name");
            builder.Property(a => a.Price).HasColumnName("Price").HasColumnType("decimal(18,0)").HasPrecision(18, 0);
            builder.Property(a => a.StockQuantity).HasColumnName("StockQuantity");
            builder.Property(a => a.Description).HasColumnName("Description");

            builder.Ignore(a => a.Publisher);
        }
    }
}
