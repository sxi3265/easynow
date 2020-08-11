// <auto-generated />
using System;
using EasyNow.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EasyNow.Dal.Migrations
{
    [DbContext(typeof(EasyNowContext))]
    partial class EasyNowContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EasyNow.Dal.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Creator")
                        .HasColumnType("char(36)");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<DateTime>("LastOnlineTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SocketId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Updater")
                        .HasColumnType("char(36)");

                    b.Property<string>("Uuid")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("EasyNow.Dal.Entities.Script", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("blob");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Creator")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Updater")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Script");
                });

            modelBuilder.Entity("EasyNow.Dal.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("nchar(50)");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Creator")
                        .HasColumnType("char(36)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("char(32)");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Updater")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("EasyNow.Dal.Entities.UserDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Creator")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Updater")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("UserId");

                    b.ToTable("UserDevice");
                });

            modelBuilder.Entity("EasyNow.Dal.Entities.UserScript", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Creator")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ScriptId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime");

                    b.Property<Guid>("Updater")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ScriptId");

                    b.HasIndex("UserId");

                    b.ToTable("UserScript");
                });

            modelBuilder.Entity("EasyNow.Dal.Entities.UserDevice", b =>
                {
                    b.HasOne("EasyNow.Dal.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyNow.Dal.Entities.User", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EasyNow.Dal.Entities.UserScript", b =>
                {
                    b.HasOne("EasyNow.Dal.Entities.Script", "Script")
                        .WithMany()
                        .HasForeignKey("ScriptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EasyNow.Dal.Entities.User", "User")
                        .WithMany("UserScripts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
