using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentor.Migrations
{
    /// <inheritdoc />
    public partial class AddFTTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPricings",
                table: "UserPricings");

            migrationBuilder.RenameTable(
                name: "UserPricings",
                newName: "UserPricing");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPricing",
                table: "UserPricing",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPricing",
                table: "UserPricing");

            migrationBuilder.RenameTable(
                name: "UserPricing",
                newName: "UserPricings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPricings",
                table: "UserPricings",
                column: "Id");
        }
    }
}
