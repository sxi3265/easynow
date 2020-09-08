﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2020/8/26 23:13:39
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyNow.Dal
{
    /// <summary>
    /// There are no comments for WxPusherAppUserConfiguration in the schema.
    /// </summary>
    public partial class WxPusherAppUserConfiguration : IEntityTypeConfiguration<WxPusherAppUser>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<WxPusherAppUser> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<WxPusherAppUser> builder)
        {
            builder.ToTable(@"WxPusherAppUser");
            builder.Property<System.Guid>(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.CreateTime).HasColumnName(@"CreateTime").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.UpdateTime).HasColumnName(@"UpdateTime").IsRequired().ValueGeneratedOnAddOrUpdate();
            builder.Property<System.Guid>(x => x.Creator).HasColumnName(@"Creator").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(x => x.Updater).HasColumnName(@"Updater").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(@"AppId").HasColumnName(@"AppId").ValueGeneratedNever();
            builder.Property<System.Guid>(@"UserId").HasColumnName(@"UserId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasOne(x => x.WxPusherApp).WithMany(op => op.WxPusherAppUsers).IsRequired(true).HasForeignKey(@"AppId");
            builder.HasOne(x => x.WxPusherUser).WithMany(op => op.WxPusherAppUsers).IsRequired(true).HasForeignKey(@"UserId");

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<WxPusherAppUser> builder);

        #endregion
    }

}
