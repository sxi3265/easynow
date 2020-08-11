using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal.Mapping
{
    public class UserDeviceMap : BaseMap<UserDevice>
    {
        public override void Map(EntityTypeBuilder<UserDevice> builder)
        {
            base.Map(builder);
            builder.ToTable("UserDevice");
            builder.HasOne(t => t.User).WithMany(t => t.UserDevices).HasForeignKey(t => t.UserId);
            builder.HasOne(t => t.Device).WithMany().HasForeignKey(t => t.DeviceId);
        }
    }
}