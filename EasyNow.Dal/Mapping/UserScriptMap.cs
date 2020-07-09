using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal.Mapping
{
    public class UserScriptMap : BaseMap<UserScript>
    {
        public override void Map(EntityTypeBuilder<UserScript> builder)
        {
            base.Map(builder);
            builder.ToTable("UserScript");
            builder.HasOne(e => e.User).WithMany(e => e.UserScripts).HasForeignKey(e => e.UserId);
            builder.HasOne(e => e.Script).WithMany().HasForeignKey(e => e.ScriptId);
        }
    }
}