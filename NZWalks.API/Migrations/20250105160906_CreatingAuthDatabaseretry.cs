using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class CreatingAuthDatabaseretry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46a7f807-a54d-4c99-aa73-ef50b20b3bd2",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Writer", "WRITER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea171c81-0a87-47c6-b96b-f4f7e50ff467",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Reader", "READER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "46a7f807-a54d-4c99-aa73-ef50b20b3bd2",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "WRITER", null });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea171c81-0a87-47c6-b96b-f4f7e50ff467",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "READER", null });
        }
    }
}
