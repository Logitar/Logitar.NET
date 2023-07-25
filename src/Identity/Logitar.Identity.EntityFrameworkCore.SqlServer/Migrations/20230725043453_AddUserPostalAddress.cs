using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPostalAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressCountry",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressFormatted",
                table: "Users",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLocality",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressPostalCode",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressRegion",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressStreet",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressVerifiedBy",
                table: "Users",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressVerifiedById",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AddressVerifiedOn",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAddressVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedById",
                table: "Users",
                column: "AddressVerifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_AddressVerifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressCountry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressFormatted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressLocality",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressPostalCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressRegion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressStreet",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressVerifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressVerifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddressVerifiedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsAddressVerified",
                table: "Users");
        }
    }
}
