using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAccountingServer.Migrations
{
    /// <inheritdoc />
    public partial class AddNewModels11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Groups",
                newName: "PasswordSalt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Expenses",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "PasswordSalt",
                table: "Groups",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Expenses",
                newName: "Name");
        }
    }
}
