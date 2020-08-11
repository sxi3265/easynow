using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal.Mapping
{
    public abstract class BaseMap<T> : IMap where T : BaseEntity
    {
        public virtual void Map(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(t => t.Id).HasColumnType("char(36)").IsRequired().ValueGeneratedOnAdd();
            builder.Property(t => t.CreateTime).HasColumnType("datetime").IsRequired().ValueGeneratedOnAdd();
            builder.Property(t => t.UpdateTime).HasColumnType("datetime").IsRequired().ValueGeneratedOnAddOrUpdate();
            builder.Property(t => t.Creator).HasColumnType("char(36)").IsRequired();
            builder.Property(t => t.Updater).HasColumnType("char(36)").IsRequired();
        }

        public virtual void Map(ModelBuilder builder)
        {
            Map(builder.Entity<T>());
        }
    }
}