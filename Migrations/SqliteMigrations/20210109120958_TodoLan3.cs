using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApi.Migrations.SqliteMigrations
{
    public partial class TodoLan3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Secret",
                table: "TodoItems",
                newName: "DateDue");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "TodoItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserRefId",
                table: "TodoItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserRefId",
                table: "TodoItems",
                column: "UserRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Users_UserRefId",
                table: "TodoItems",
                column: "UserRefId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Users_UserRefId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_UserRefId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "UserRefId",
                table: "TodoItems");

            migrationBuilder.RenameColumn(
                name: "DateDue",
                table: "TodoItems",
                newName: "Secret");
        }
    }
}
