using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BetCommerce.DataAccess.Migrations
{
    public partial class initEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(maxLength: 255, nullable: true),
                    MoreInfo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductBarcode = table.Column<string>(maxLength: 150, nullable: false),
                    ProductName = table.Column<string>(maxLength: 255, nullable: false),
                    Category = table.Column<string>(maxLength: 225, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CurrentQuantity = table.Column<double>(nullable: false),
                    SellingPrice = table.Column<double>(nullable: false),
                    ImageFile = table.Column<string>(type: "text", nullable: true),
                    RegisteredDateUtc = table.Column<DateTime>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductStockCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(maxLength: 50, nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Narration = table.Column<string>(maxLength: 250, nullable: true),
                    RefNo = table.Column<string>(maxLength: 90, nullable: true),
                    OpeningQuantity = table.Column<double>(nullable: false),
                    StockIn = table.Column<double>(nullable: false),
                    StockOut = table.Column<double>(nullable: false),
                    ClosingQuantity = table.Column<double>(nullable: false),
                    TransactionDateUtc = table.Column<DateTime>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStockCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FullName = table.Column<string>(maxLength: 255, nullable: true),
                    MobilePhone = table.Column<string>(maxLength: 90, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 255, nullable: false),
                    InvalidLogins = table.Column<int>(nullable: false),
                    ProfileImage = table.Column<string>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    OauthKey = table.Column<string>(maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    IsMobilePhoneConfirmed = table.Column<bool>(nullable: false),
                    DateRegisteredUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderRef = table.Column<string>(maxLength: 250, nullable: true),
                    UserAccountId = table.Column<int>(nullable: true),
                    UserAccountName = table.Column<string>(nullable: true),
                    TotalItems = table.Column<int>(nullable: false),
                    SubTotal = table.Column<double>(nullable: false),
                    Tax = table.Column<double>(nullable: false),
                    TaxRate = table.Column<double>(nullable: false),
                    DeliveryCost = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    DueAmount = table.Column<double>(nullable: false),
                    PaidAmount = table.Column<double>(nullable: false),
                    PayMethod = table.Column<string>(maxLength: 240, nullable: true),
                    OrderDateUtc = table.Column<DateTime>(nullable: false),
                    LastStatusChangeDateUtc = table.Column<DateTime>(nullable: false),
                    OrderNote = table.Column<string>(type: "text", nullable: true),
                    OrderStatus = table.Column<string>(maxLength: 50, nullable: true),
                    UserAccountId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_UserAccounts_UserAccountId1",
                        column: x => x.UserAccountId1,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductBarcode = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    TotalCost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserAccountId1",
                table: "Orders",
                column: "UserAccountId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductStockCards");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
