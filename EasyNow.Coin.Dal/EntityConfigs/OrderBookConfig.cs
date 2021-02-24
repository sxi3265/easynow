using EasyNow.Coin.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EasyNow.Coin.Dal.EntityConfigs
{
    public class OrderBookConfig:BaseConfig<OrderBook>
    {
        public override void Configure(EntityTypeBuilder<OrderBook> builder)
        {
            base.Configure(builder);

            builder.ToTable("T_Spot_OrderBook");
            builder.Property(t => t.Type).HasColumnType("int").IsRequired();
            builder.Property(t => t.Symbol).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(t => t.Price).IsRequired();
            builder.Property(t => t.Count).IsRequired();
            builder.Property(t => t.OrderCount).IsRequired();
        }
    }
}