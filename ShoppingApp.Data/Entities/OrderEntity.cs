using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingApp.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public UserEntity Customer { get; set; }

        public ICollection<OrderProductEntity> Products { get; set; }

    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.Ignore(x => x.Id); //ıd propertysini görmezden geldik, tabloya aktarılmayacak
            builder.HasKey("Id", "CustomerId"); //composite key oluşturup yeni primary key olarak atıldı
            base.Configure(builder);
        }
    }
}

 
    