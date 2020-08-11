using System.Linq;
using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal.Mapping
{
    public class ScriptMap : BaseMap<Script>
    {
        public override void Map(EntityTypeBuilder<Script> builder)
        {
            base.Map(builder);
            builder.ToTable("Script");
            builder.Property(t => t.Name).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(t => t.Code).HasColumnType("varchar(200)").IsRequired();
            builder.Property(t => t.Content).HasColumnType("blob").IsRequired();
        }
    }
}