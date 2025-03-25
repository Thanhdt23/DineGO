using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DineGO_Api.Migrations
{
    public partial class InitialDB_V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    ad_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ad_username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ad_password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ad_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ad_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ad_birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ad_image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.ad_id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    cate_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    res_id = table.Column<int>(type: "int", nullable: false),
                    cate_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cate_description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.cate_id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    cus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    cus_password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cus_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cus_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cus_address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    cus_birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cus_gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cus_image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.cus_id);
                });

            migrationBuilder.CreateTable(
                name: "restaurants",
                columns: table => new
                {
                    res_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    res_username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    res_password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    res_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    res_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    res_address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    res_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    res_information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    res_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    res_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    res_discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    res_images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cate_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restaurants", x => x.res_id);
                    table.ForeignKey(
                        name: "FK_restaurants_categories_cate_id",
                        column: x => x.cate_id,
                        principalTable: "categories",
                        principalColumn: "cate_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    blog_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    res_id = table.Column<int>(type: "int", nullable: false),
                    blog_title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    blog_information = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blog_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    blog_image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.blog_id);
                    table.ForeignKey(
                        name: "FK_blogs_restaurants_res_id",
                        column: x => x.res_id,
                        principalTable: "restaurants",
                        principalColumn: "res_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    re_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_id = table.Column<int>(type: "int", nullable: false),
                    res_id = table.Column<int>(type: "int", nullable: false),
                    re_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    re_quantity = table.Column<int>(type: "int", nullable: false),
                    re_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    re_note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.re_id);
                    table.ForeignKey(
                        name: "FK_reservations_customers_cus_id",
                        column: x => x.cus_id,
                        principalTable: "customers",
                        principalColumn: "cus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_restaurants_res_id",
                        column: x => x.res_id,
                        principalTable: "restaurants",
                        principalColumn: "res_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    noti_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_id = table.Column<int>(type: "int", nullable: false),
                    re_id = table.Column<int>(type: "int", nullable: false),
                    noti_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    noti_content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    noti_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    noti_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    noti_status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.noti_id);
                    table.ForeignKey(
                        name: "FK_notifications_customers_cus_id",
                        column: x => x.cus_id,
                        principalTable: "customers",
                        principalColumn: "cus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notifications_reservations_re_id",
                        column: x => x.re_id,
                        principalTable: "reservations",
                        principalColumn: "re_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogs_res_id",
                table: "blogs",
                column: "res_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_cus_id",
                table: "notifications",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_re_id",
                table: "notifications",
                column: "re_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_cus_id",
                table: "reservations",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_res_id",
                table: "reservations",
                column: "res_id");

            migrationBuilder.CreateIndex(
                name: "IX_restaurants_cate_id",
                table: "restaurants",
                column: "cate_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "blogs");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "restaurants");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
