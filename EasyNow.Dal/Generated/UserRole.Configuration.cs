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
    /// There are no comments for UserRoleConfiguration in the schema.
    /// </summary>
    public partial class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<UserRole> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(@"UserRole");
            builder.Property<System.Guid>(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.CreateTime).HasColumnName(@"CreateTime").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.UpdateTime).HasColumnName(@"UpdateTime").IsRequired().ValueGeneratedOnAddOrUpdate();
            builder.Property<System.Guid>(x => x.Creator).HasColumnName(@"Creator").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(x => x.Updater).HasColumnName(@"Updater").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(@"UserId").HasColumnName(@"UserId").ValueGeneratedNever();
            builder.Property<System.Guid>(@"RoleId").HasColumnName(@"RoleId").ValueGeneratedNever();
            builder.HasKey(@"Id");
            builder.HasOne(x => x.User).WithMany(op => op.UserRoles).IsRequired(true).HasForeignKey(@"UserId");
            builder.HasOne(x => x.Role).WithMany(op => op.UserRoles).IsRequired(true).HasForeignKey(@"RoleId");

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<UserRole> builder);

        #endregion
    }

}
