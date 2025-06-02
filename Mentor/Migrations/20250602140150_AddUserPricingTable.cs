using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mentor.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPricingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedinUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPricings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricingId = table.Column<int>(type: "int", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPricings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPricings");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "LinkedinUrl",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Trainers");
        }
    }
}
