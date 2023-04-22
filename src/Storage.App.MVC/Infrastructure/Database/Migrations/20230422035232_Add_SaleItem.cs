using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.App.MVC.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_SaleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Products_ProductEntityId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ProductEntityId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "Sales");

            migrationBuilder.AddColumn<Guid>(
                name: "EnterpriseId",
                table: "SaleItemEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SaleItemEntity_EnterpriseId",
                table: "SaleItemEntity",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItemEntity_Enterprises_EnterpriseId",
                table: "SaleItemEntity",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItemEntity_Enterprises_EnterpriseId",
                table: "SaleItemEntity");

            migrationBuilder.DropIndex(
                name: "IX_SaleItemEntity_EnterpriseId",
                table: "SaleItemEntity");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "SaleItemEntity");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductEntityId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductEntityId",
                table: "Sales",
                column: "ProductEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Products_ProductEntityId",
                table: "Sales",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
