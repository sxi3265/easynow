﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2020/8/22 21:34:31
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
    /// There are no comments for WxPusherUserConfiguration in the schema.
    /// </summary>
    public partial class WxPusherUserConfiguration : IEntityTypeConfiguration<WxPusherUser>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<WxPusherUser> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<WxPusherUser> builder)
        {
            builder.ToTable(@"WxPusherUser");
            builder.Property<System.Guid>(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.CreateTime).HasColumnName(@"CreateTime").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.UpdateTime).HasColumnName(@"UpdateTime").IsRequired().ValueGeneratedOnAddOrUpdate();
            builder.Property<System.Guid>(x => x.Creator).HasColumnName(@"Creator").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(x => x.Updater).HasColumnName(@"Updater").IsRequired().ValueGeneratedNever();
            builder.Property<string>(x => x.Uid).HasColumnName(@"Uid").IsRequired().ValueGeneratedNever().HasMaxLength(200);
            builder.Property<string>(x => x.NickName).HasColumnName(@"NickName").IsRequired().ValueGeneratedNever().HasMaxLength(200);
            builder.Property<string>(x => x.HeadImg).HasColumnName(@"HeadImg").IsRequired().ValueGeneratedNever().HasMaxLength(200);
            builder.Property<bool>(x => x.Enable).HasColumnName(@"Enable").IsRequired().ValueGeneratedNever();
            builder.Property<System.DateTime>(x => x.SubTime).HasColumnName(@"SubTime").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(@"AppId").HasColumnName(@"AppId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasOne(x => x.WxPusherApp).WithMany(op => op.WxPusherUsers).IsRequired(true).HasForeignKey(@"AppId");

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<WxPusherUser> builder);

        #endregion
    }

}
