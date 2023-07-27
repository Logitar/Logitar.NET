using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UniqueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggregateId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_AggregateId",
                table: "Roles",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedOn",
                table: "Roles",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DisplayName",
                table: "Roles",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId",
                table: "Roles",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId_UniqueNameNormalized",
                table: "Roles",
                columns: new[] { "TenantId", "UniqueNameNormalized" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UniqueName",
                table: "Roles",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedById",
                table: "Roles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedOn",
                table: "Roles",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Version",
                table: "Roles",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
