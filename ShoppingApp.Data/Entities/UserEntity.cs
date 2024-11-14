using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingApp.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingApp.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime? BirthDate { get; set; }
        public UserType UserType { get; set; }


        //Relational Property
        public ICollection<OrderEntity> Orders { get; set; }

    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(40);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(40);
            builder.Property(x => x.BirthDate).IsRequired(false);
            base.Configure(builder);
        }
    }
}
