using Microsoft.EntityFrameworkCore.Migrations;

namespace DineGO_Api.Migrations
{
    public partial class IntialDB_V101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "res_id",
                table: "categories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "res_id",
                table: "categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
