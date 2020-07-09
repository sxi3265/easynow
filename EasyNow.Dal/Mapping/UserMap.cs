using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal.Mapping
{
    public class UserMap:BaseMap<User>
    {
        public override void Map(EntityTypeBuilder<User> builder)
        {
            base.Map(builder);
            builder.ToTable("User");
            builder.Property(t => t.Account).HasColumnType("nchar(50)").IsRequired();
            builder.Property(t => t.Password).HasColumnType("char(32)").IsRequired();
        }
    }
}