using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPhoneVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneCountryCode",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneE164Formatted",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneExtension",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneVerifiedBy",
                table: "Users",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneVerifiedById",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhoneVerifiedOn",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedById",
                table: "Users",
                column: "PhoneVerifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneVerifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsPhoneVerified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneCountryCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneE164Formatted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneExtension",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneVerifiedBy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneVerifiedById",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneVerifiedOn",
                table: "Users");
        }
    }
}
