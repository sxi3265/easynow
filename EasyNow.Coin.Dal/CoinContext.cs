using System;
using System.Collections.Generic;
using System.Text;
using EasyNow.Coin.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Coin.Dal
{
    public class CoinContext:DbContext
    {
        public DbSet<OrderBook> OrderBook { get; set; }

        public CoinContext(DbContextOptions<CoinContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
