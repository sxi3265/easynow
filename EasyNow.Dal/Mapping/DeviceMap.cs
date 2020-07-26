using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EasyNow.Dal.Mapping
{
    public class DeviceMap : BaseMap<Device>
    {
        public override void Map(EntityTypeBuilder<Device> builder)
        {
            base.Map(builder);
            builder.ToTable("Device");
            builder.Property(t => t.Name).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(t => t.Ip).HasColumnType("varchar(15)").IsRequired();
            builder.Property(t => t.Status).HasColumnType("int").IsRequired().HasConversion(new EnumToNumberConverter<DeviceStatus,int>());
            builder.Property(t => t.LastOnlineTime).HasColumnType("datetime").IsRequired();
            builder.Property(t => t.SocketId).HasColumnType("char(36)").IsRequired();
            builder.Property(t => t.Uuid).HasColumnType("varchar(100)").IsRequired();
        }
    }
}