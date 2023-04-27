using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.App.MVC.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_ActivityHistory_ColumnActivityAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityAction",
                table: "ActivityHistory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityAction",
                table: "ActivityHistory");
        }
    }
}
