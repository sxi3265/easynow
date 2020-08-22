﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2020/8/22 16:57:40
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
    /// There are no comments for PrivilegeConfiguration in the schema.
    /// </summary>
    public partial class PrivilegeConfiguration : IEntityTypeConfiguration<Privilege>
    {
        /// <summary>
        /// There are no comments for Configure(EntityTypeBuilder<Privilege> builder) method in the schema.
        /// </summary>
        public void Configure(EntityTypeBuilder<Privilege> builder)
        {
            builder.ToTable(@"Privilege");
            builder.Property<System.Guid>(x => x.Id).HasColumnName(@"Id").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.CreateTime).HasColumnName(@"CreateTime").IsRequired().ValueGeneratedOnAdd();
            builder.Property<System.DateTime>(x => x.UpdateTime).HasColumnName(@"UpdateTime").IsRequired().ValueGeneratedOnAddOrUpdate();
            builder.Property<System.Guid>(x => x.Creator).HasColumnName(@"Creator").IsRequired().ValueGeneratedNever();
            builder.Property<System.Guid>(x => x.Updater).HasColumnName(@"Updater").IsRequired().ValueGeneratedNever();
            builder.Property<string>(x => x.Code).HasColumnName(@"Code").IsRequired().ValueGeneratedNever().HasMaxLength(200);
            builder.Property<string>(x => x.Name).HasColumnName(@"Name").IsRequired().ValueGeneratedNever().HasMaxLength(200);
            builder.HasKey(@"Id");

            CustomizeConfiguration(builder);
        }

        #region Partial Methods

        partial void CustomizeConfiguration(EntityTypeBuilder<Privilege> builder);

        #endregion
    }

}
