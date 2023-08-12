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
            CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            Version = table.Column<long>(type: "bigint", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Roles", x => x.RoleId);
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
            PasswordChangedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            PasswordChangedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            DisabledBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            DisabledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            IsDisabled = table.Column<bool>(type: "bit", nullable: false),
            AuthenticatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            AddressStreet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            AddressLocality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            AddressRegion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            AddressPostalCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            AddressCountry = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            AddressFormatted = table.Column<string>(type: "nvarchar(1536)", maxLength: 1536, nullable: true),
            AddressVerifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            AddressVerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            IsAddressVerified = table.Column<bool>(type: "bit", nullable: false),
            EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            EmailAddressNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            EmailVerifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            EmailVerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
            PhoneCountryCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
            PhoneNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
            PhoneExtension = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
            PhoneE164Formatted = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
            PhoneVerifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            PhoneVerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false),
            IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
            FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            MiddleName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            FullName = table.Column<string>(type: "nvarchar(768)", maxLength: 768, nullable: true),
            Nickname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            Birthdate = table.Column<DateTime>(type: "datetime2", nullable: true),
            Gender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            Locale = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
            TimeZone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            Picture = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
            Profile = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
            Website = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
            CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            AggregateId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            Version = table.Column<long>(type: "bigint", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Users", x => x.UserId);
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
            SignedOutBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            SignedOutOn = table.Column<DateTime>(type: "datetime2", nullable: true),
            CustomAttributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            AggregateId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
            UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
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
          name: "IX_Roles_AggregateId",
          table: "Roles",
          column: "AggregateId",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_Roles_CreatedBy",
          table: "Roles",
          column: "CreatedBy");

      migrationBuilder.CreateIndex(
          name: "IX_Roles_CreatedOn",
          table: "Roles",
          column: "CreatedOn");

      migrationBuilder.CreateIndex(
          name: "IX_Roles_DisplayName",
          table: "Roles",
          column: "DisplayName");

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
          name: "IX_Roles_UpdatedBy",
          table: "Roles",
          column: "UpdatedBy");

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
          name: "IX_Sessions_CreatedBy",
          table: "Sessions",
          column: "CreatedBy");

      migrationBuilder.CreateIndex(
          name: "IX_Sessions_CreatedOn",
          table: "Sessions",
          column: "CreatedOn");

      migrationBuilder.CreateIndex(
          name: "IX_Sessions_IsActive",
          table: "Sessions",
          column: "IsActive");

      migrationBuilder.CreateIndex(
          name: "IX_Sessions_IsPersistent",
          table: "Sessions",
          column: "IsPersistent");

      migrationBuilder.CreateIndex(
          name: "IX_Sessions_SignedOutBy",
          table: "Sessions",
          column: "SignedOutBy");

      migrationBuilder.CreateIndex(
          name: "IX_Sessions_SignedOutOn",
          table: "Sessions",
          column: "SignedOutOn");

      migrationBuilder.CreateIndex(
          name: "IX_Sessions_UpdatedBy",
          table: "Sessions",
          column: "UpdatedBy");

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
          name: "IX_UserRoles_RoleId",
          table: "UserRoles",
          column: "RoleId");

      migrationBuilder.CreateIndex(
          name: "IX_Users_AddressFormatted",
          table: "Users",
          column: "AddressFormatted");

      migrationBuilder.CreateIndex(
          name: "IX_Users_AddressVerifiedBy",
          table: "Users",
          column: "AddressVerifiedBy");

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
          name: "IX_Users_CreatedBy",
          table: "Users",
          column: "CreatedBy");

      migrationBuilder.CreateIndex(
          name: "IX_Users_CreatedOn",
          table: "Users",
          column: "CreatedOn");

      migrationBuilder.CreateIndex(
          name: "IX_Users_DisabledBy",
          table: "Users",
          column: "DisabledBy");

      migrationBuilder.CreateIndex(
          name: "IX_Users_DisabledOn",
          table: "Users",
          column: "DisabledOn");

      migrationBuilder.CreateIndex(
          name: "IX_Users_EmailAddress",
          table: "Users",
          column: "EmailAddress");

      migrationBuilder.CreateIndex(
          name: "IX_Users_EmailVerifiedBy",
          table: "Users",
          column: "EmailVerifiedBy");

      migrationBuilder.CreateIndex(
          name: "IX_Users_FirstName",
          table: "Users",
          column: "FirstName");

      migrationBuilder.CreateIndex(
          name: "IX_Users_FullName",
          table: "Users",
          column: "FullName");

      migrationBuilder.CreateIndex(
          name: "IX_Users_Gender",
          table: "Users",
          column: "Gender");

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
          name: "IX_Users_Locale",
          table: "Users",
          column: "Locale");

      migrationBuilder.CreateIndex(
          name: "IX_Users_MiddleName",
          table: "Users",
          column: "MiddleName");

      migrationBuilder.CreateIndex(
          name: "IX_Users_Nickname",
          table: "Users",
          column: "Nickname");

      migrationBuilder.CreateIndex(
          name: "IX_Users_PasswordChangedBy",
          table: "Users",
          column: "PasswordChangedBy");

      migrationBuilder.CreateIndex(
          name: "IX_Users_PasswordChangedOn",
          table: "Users",
          column: "PasswordChangedOn");

      migrationBuilder.CreateIndex(
          name: "IX_Users_PhoneE164Formatted",
          table: "Users",
          column: "PhoneE164Formatted");

      migrationBuilder.CreateIndex(
          name: "IX_Users_PhoneVerifiedBy",
          table: "Users",
          column: "PhoneVerifiedBy");

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
          name: "IX_Users_TimeZone",
          table: "Users",
          column: "TimeZone");

      migrationBuilder.CreateIndex(
          name: "IX_Users_UniqueName",
          table: "Users",
          column: "UniqueName");

      migrationBuilder.CreateIndex(
          name: "IX_Users_UpdatedBy",
          table: "Users",
          column: "UpdatedBy");

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
          name: "Sessions");

      migrationBuilder.DropTable(
          name: "UserRoles");

      migrationBuilder.DropTable(
          name: "Roles");

      migrationBuilder.DropTable(
          name: "Users");
    }
  }
}
