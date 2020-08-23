﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2020/8/23 13:52:49
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EasyNow.Dal
{

    public partial class EasyNowContext : DbContext
    {

        public EasyNowContext() :
            base()
        {
            OnCreated();
        }

        public EasyNowContext(DbContextOptions<EasyNowContext> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<User> User
        {
            get;
            set;
        }

        public virtual DbSet<Role> Role
        {
            get;
            set;
        }

        public virtual DbSet<Privilege> Privilege
        {
            get;
            set;
        }

        public virtual DbSet<UserRole> UserRole
        {
            get;
            set;
        }

        public virtual DbSet<RolePrivilege> RolePrivilege
        {
            get;
            set;
        }

        public virtual DbSet<Menu> Menu
        {
            get;
            set;
        }

        public virtual DbSet<RoleMenu> RoleMenu
        {
            get;
            set;
        }

        public virtual DbSet<Device> Device
        {
            get;
            set;
        }

        public virtual DbSet<UserDevice> UserDevice
        {
            get;
            set;
        }

        public virtual DbSet<Service> Service
        {
            get;
            set;
        }

        public virtual DbSet<ServiceCategory> ServiceCategory
        {
            get;
            set;
        }

        public virtual DbSet<UserService> UserService
        {
            get;
            set;
        }

        public virtual DbSet<AutoJsService> AutoJsService
        {
            get;
            set;
        }

        public virtual DbSet<WxPusherUser> WxPusherUser
        {
            get;
            set;
        }

        public virtual DbSet<WxPusherApp> WxPusherApp
        {
            get;
            set;
        }

        public virtual DbSet<WxPusherAppUser> WxPusherAppUser
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new UserConfiguration());
            modelBuilder.ApplyConfiguration<Role>(new RoleConfiguration());
            modelBuilder.ApplyConfiguration<Privilege>(new PrivilegeConfiguration());
            modelBuilder.ApplyConfiguration<UserRole>(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration<RolePrivilege>(new RolePrivilegeConfiguration());
            modelBuilder.ApplyConfiguration<Menu>(new MenuConfiguration());
            modelBuilder.ApplyConfiguration<RoleMenu>(new RoleMenuConfiguration());
            modelBuilder.ApplyConfiguration<Device>(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration<UserDevice>(new UserDeviceConfiguration());
            modelBuilder.ApplyConfiguration<Service>(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration<ServiceCategory>(new ServiceCategoryConfiguration());
            modelBuilder.ApplyConfiguration<UserService>(new UserServiceConfiguration());
            modelBuilder.ApplyConfiguration<AutoJsService>(new AutoJsServiceConfiguration());
            modelBuilder.ApplyConfiguration<WxPusherUser>(new WxPusherUserConfiguration());
            modelBuilder.ApplyConfiguration<WxPusherApp>(new WxPusherAppConfiguration());
            modelBuilder.ApplyConfiguration<WxPusherAppUser>(new WxPusherAppUserConfiguration());
            CustomizeMapping(ref modelBuilder);
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
