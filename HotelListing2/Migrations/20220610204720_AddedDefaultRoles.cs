using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelListing2.Migrations
{
    public partial class AddedDefaultRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6b60f052-3002-4ec3-9eea-cb203a8dfaf4", "88249287-c5e4-4b12-b09f-9afe25a5269e", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba8cb4d3-c357-42ac-bddc-5cd6d381c21d", "2a40db12-d839-4e8c-a5cd-7dfb76611df2", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b60f052-3002-4ec3-9eea-cb203a8dfaf4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba8cb4d3-c357-42ac-bddc-5cd6d381c21d");
        }
    }
}
