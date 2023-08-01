using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    ApiKeyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Secret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthenticatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_ApiKeys", x => x.ApiKeyId);
                });

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
                    CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "TokenBlacklist",
                columns: table => new
                {
                    BlacklistedTokenId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenBlacklist", x => x.BlacklistedTokenId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UniqueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HasPassword = table.Column<bool>(type: "bit", nullable: false),
                    PasswordChangedById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PasswordChangedBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    PasswordChangedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisabledById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DisabledBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    DisabledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    AddressStreet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressLocality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressCountry = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressRegion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressPostalCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressFormatted = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    AddressVerifiedById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AddressVerifiedBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    AddressVerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAddressVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmailAddressNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmailVerifiedById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmailVerifiedBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    EmailVerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    PhoneCountryCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneExtension = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneE164Formatted = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneVerifiedById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneVerifiedBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    PhoneVerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    AuthenticatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Nickname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Locale = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Profile = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeyRoles",
                columns: table => new
                {
                    ApiKeyId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeyRoles", x => new { x.ApiKeyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApiKeyRoles_ApiKeys_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalTable: "ApiKeys",
                        principalColumn: "ApiKeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiKeyRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalIdentifiers",
                columns: table => new
                {
                    ExternalIdentifierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ValueNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
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
                    table.PrimaryKey("PK_ExternalIdentifiers", x => x.ExternalIdentifierId);
                    table.ForeignKey(
                        name: "FK_ExternalIdentifiers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsPersistent = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SignedOutById = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SignedOutBy = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    SignedOutOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeyRoles_RoleId",
                table: "ApiKeyRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_AggregateId",
                table: "ApiKeys",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_CreatedById",
                table: "ApiKeys",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_CreatedOn",
                table: "ApiKeys",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_ExpiresOn",
                table: "ApiKeys",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_TenantId",
                table: "ApiKeys",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Title",
                table: "ApiKeys",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UpdatedById",
                table: "ApiKeys",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_UpdatedOn",
                table: "ApiKeys",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeys_Version",
                table: "ApiKeys",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_CreatedById",
                table: "ExternalIdentifiers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_Id",
                table: "ExternalIdentifiers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_TenantId_Key_ValueNormalized",
                table: "ExternalIdentifiers",
                columns: new[] { "TenantId", "Key", "ValueNormalized" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_UpdatedById",
                table: "ExternalIdentifiers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalIdentifiers_UserId",
                table: "ExternalIdentifiers",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AggregateId",
                table: "Sessions",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CreatedById",
                table: "Sessions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CreatedOn",
                table: "Sessions",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SignedOutById",
                table: "Sessions",
                column: "SignedOutById");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UpdatedById",
                table: "Sessions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UpdatedOn",
                table: "Sessions",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Version",
                table: "Sessions",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_TokenBlacklist_ExpiresOn",
                table: "TokenBlacklist",
                column: "ExpiresOn");

            migrationBuilder.CreateIndex(
                name: "IX_TokenBlacklist_Id",
                table: "TokenBlacklist",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressFormatted",
                table: "Users",
                column: "AddressFormatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressVerifiedById",
                table: "Users",
                column: "AddressVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AggregateId",
                table: "Users",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthenticatedOn",
                table: "Users",
                column: "AuthenticatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Birthdate",
                table: "Users",
                column: "Birthdate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedOn",
                table: "Users",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisabledById",
                table: "Users",
                column: "DisabledById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisabledOn",
                table: "Users",
                column: "DisabledOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerifiedById",
                table: "Users",
                column: "EmailVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullName",
                table: "Users",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HasPassword",
                table: "Users",
                column: "HasPassword");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsConfirmed",
                table: "Users",
                column: "IsConfirmed");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDisabled",
                table: "Users",
                column: "IsDisabled");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName",
                table: "Users",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MiddleName",
                table: "Users",
                column: "MiddleName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordChangedById",
                table: "Users",
                column: "PasswordChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordChangedOn",
                table: "Users",
                column: "PasswordChangedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneE164Formatted",
                table: "Users",
                column: "PhoneE164Formatted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneVerifiedById",
                table: "Users",
                column: "PhoneVerifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_EmailAddressNormalized",
                table: "Users",
                columns: new[] { "TenantId", "EmailAddressNormalized" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_UniqueNameNormalized",
                table: "Users",
                columns: new[] { "TenantId", "UniqueNameNormalized" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UniqueName",
                table: "Users",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedOn",
                table: "Users",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Version",
                table: "Users",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeyRoles");

            migrationBuilder.DropTable(
                name: "ExternalIdentifiers");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "TokenBlacklist");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ApiKeys");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
