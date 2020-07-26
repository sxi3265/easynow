using System;
using System.Collections.Generic;
using Autofac;
using EasyNow.Dal.Entities;
using EasyNow.Dal.Mapping;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Dal
{
    public class EasyNowContext:DbContext
    {
        private readonly ILifetimeScope _scope;

        public EasyNowContext(ILifetimeScope scope,DbContextOptions<EasyNowContext> options):base(options)
        {
            _scope = scope;
        }

        public DbSet<User> User { get; set; }
        public DbSet<Script> Script { get; set; }
        public DbSet<UserScript> UserScript { get; set; }
        public DbSet<Device> Device { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var map in _scope.Resolve<IEnumerable<IMap>>())
            {
                map.Map(modelBuilder);
            }
        }
    }
}