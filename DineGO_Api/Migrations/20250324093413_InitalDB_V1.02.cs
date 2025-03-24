using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DineGO_Api.Migrations
{
    public partial class InitalDB_V102 : Migration
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
                    cus_image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cus_isKYI = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.cus_id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    noti_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_id = table.Column<int>(type: "int", nullable: false),
                    noti_title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    noti_content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    noti_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    noti_date = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "restaurantOwners",
                columns: table => new
                {
                    resOwner_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_id = table.Column<int>(type: "int", nullable: false),
                    resOwner_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resOwner_createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    resOwner_isAuthorize = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restaurantOwners", x => x.resOwner_id);
                    table.ForeignKey(
                        name: "FK_restaurantOwners_customers_cus_id",
                        column: x => x.cus_id,
                        principalTable: "customers",
                        principalColumn: "cus_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    blog_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resOwner_id = table.Column<int>(type: "int", nullable: false),
                    blog_title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    blog_information = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blog_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    blog_image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blogs", x => x.blog_id);
                    table.ForeignKey(
                        name: "FK_blogs_restaurantOwners_resOwner_id",
                        column: x => x.resOwner_id,
                        principalTable: "restaurantOwners",
                        principalColumn: "resOwner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "restaurants",
                columns: table => new
                {
                    res_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cate_id = table.Column<int>(type: "int", nullable: false),
                    resOwner_id = table.Column<int>(type: "int", nullable: false),
                    res_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    res_address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    res_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    res_information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    res_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    res_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    res_discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    res_images = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_restaurants_restaurantOwners_resOwner_id",
                        column: x => x.resOwner_id,
                        principalTable: "restaurantOwners",
                        principalColumn: "resOwner_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    reser_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_id = table.Column<int>(type: "int", nullable: false),
                    res_id = table.Column<int>(type: "int", nullable: false),
                    reser_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reser_quantity = table.Column<int>(type: "int", nullable: false),
                    reser_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    reser_note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.reser_id);
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
                        principalColumn: "res_id");
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    pay_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cus_id = table.Column<int>(type: "int", nullable: false),
                    reser_id = table.Column<int>(type: "int", nullable: false),
                    pay_price = table.Column<double>(type: "float", nullable: false),
                    pay_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pay_createdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.pay_id);
                    table.ForeignKey(
                        name: "FK_payments_customers_cus_id",
                        column: x => x.cus_id,
                        principalTable: "customers",
                        principalColumn: "cus_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_reservations_reser_id",
                        column: x => x.reser_id,
                        principalTable: "reservations",
                        principalColumn: "reser_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogs_resOwner_id",
                table: "blogs",
                column: "resOwner_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_cus_id",
                table: "notifications",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_cus_id",
                table: "payments",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_reser_id",
                table: "payments",
                column: "reser_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_cus_id",
                table: "reservations",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_res_id",
                table: "reservations",
                column: "res_id");

            migrationBuilder.CreateIndex(
                name: "IX_restaurantOwners_cus_id",
                table: "restaurantOwners",
                column: "cus_id");

            migrationBuilder.CreateIndex(
                name: "IX_restaurants_cate_id",
                table: "restaurants",
                column: "cate_id");

            migrationBuilder.CreateIndex(
                name: "IX_restaurants_resOwner_id",
                table: "restaurants",
                column: "resOwner_id");
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
                name: "payments");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "restaurants");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "restaurantOwners");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
