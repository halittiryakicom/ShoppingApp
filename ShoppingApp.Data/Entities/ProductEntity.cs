using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ShoppingApp.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public ICollection<OrderProductEntity> Orders { get; set; }
    }

    public class ProductConfiguration : BaseConfiguration<ProductEntity>
    {
        public override void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.Property(x => x.ProductName).IsRequired().HasMaxLength(80);

            base.Configure(builder);
        }
    }
}
