using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ShoppingApp.Data.Entities
{
    //Hangi ürünleri hangi siparişlerde yer aldı
    public class OrderProductEntity : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        //Relational Property
        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }
    }

    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            builder.Ignore(x => x.Id); //ıd propertysini görmezden geldik, tabloya aktarılmayacak
            builder.HasKey("ProductId", "OrderId"); //composite key oluşturup yeni primary key olarak atıldı
            base.Configure(builder);
        }
    }


}
