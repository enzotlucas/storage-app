using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.App.MVC.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_ObjectId_ActivityHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ObjectId",
                table: "ActivityHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "ActivityHistory");
        }
    }
}
