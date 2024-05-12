using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAccountingServer.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isBlocked",
                table: "GroupMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBlocked",
                table: "GroupMembers");
        }
    }
}
