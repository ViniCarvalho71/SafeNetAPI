using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeNetAPI.Migrations
{
    /// <inheritdoc />
    public partial class Azure_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsMalicious",
                table: "Request",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMalicious",
                table: "Request");
        }
    }
}
