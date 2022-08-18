using Microsoft.EntityFrameworkCore.Migrations;

namespace BetCommerce.DataAccess.Migrations
{
    public partial class orderEntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserAccounts_UserAccountId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserAccountId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserAccountId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserAccountName",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "UserAccountId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerEmail",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserAccountId",
                table: "Orders",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserAccounts_UserAccountId",
                table: "Orders",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserAccounts_UserAccountId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserAccountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerEmail",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "UserAccountId",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccountId1",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAccountName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserAccountId1",
                table: "Orders",
                column: "UserAccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserAccounts_UserAccountId1",
                table: "Orders",
                column: "UserAccountId1",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
