using Microsoft.EntityFrameworkCore.Migrations;

namespace kursach.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    NameOfProduct = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Price = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.NameOfProduct);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "SoldProducts",
                columns: table => new
                {
                    SoldProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOfProduct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfSale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOfSale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNameOfProduct = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    UserLogin = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldProducts", x => x.SoldProductID);
                    table.ForeignKey(
                        name: "FK_SoldProducts_Products_ProductNameOfProduct",
                        column: x => x.ProductNameOfProduct,
                        principalTable: "Products",
                        principalColumn: "NameOfProduct",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoldProducts_Users_UserLogin",
                        column: x => x.UserLogin,
                        principalTable: "Users",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_ProductNameOfProduct",
                table: "SoldProducts",
                column: "ProductNameOfProduct");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_UserLogin",
                table: "SoldProducts",
                column: "UserLogin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldProducts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
