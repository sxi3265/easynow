using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyNow.Dal.Migrations
{
    public partial class ChangeWxPusher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WxPusherUser_WxPusherApp_AppId",
                table: "WxPusherUser");

            migrationBuilder.DropIndex(
                name: "IX_WxPusherUser_AppId",
                table: "WxPusherUser");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "WxPusherUser");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "WxPusherApp",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "WxPusherAppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Creator = table.Column<Guid>(nullable: false),
                    Updater = table.Column<Guid>(nullable: false),
                    AppId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WxPusherAppUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WxPusherAppUser_WxPusherApp_AppId",
                        column: x => x.AppId,
                        principalTable: "WxPusherApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WxPusherAppUser_WxPusherUser_UserId",
                        column: x => x.UserId,
                        principalTable: "WxPusherUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WxPusherAppUser_AppId",
                table: "WxPusherAppUser",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_WxPusherAppUser_UserId",
                table: "WxPusherAppUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WxPusherAppUser");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "WxPusherApp");

            migrationBuilder.AddColumn<Guid>(
                name: "AppId",
                table: "WxPusherUser",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WxPusherUser_AppId",
                table: "WxPusherUser",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_WxPusherUser_WxPusherApp_AppId",
                table: "WxPusherUser",
                column: "AppId",
                principalTable: "WxPusherApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
