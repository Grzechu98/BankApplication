using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApplication.Migrations
{
    public partial class ioperationAndOperationModelsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "ExternalAccountNumber",
                table: "ExternalOperations");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Operations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientAccountNumber",
                table: "Operations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientAccountNumber",
                table: "ExternalOperations",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations",
                column: "SenderId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "RecipientAccountNumber",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "RecipientAccountNumber",
                table: "ExternalOperations");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Operations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "ExternalAccountNumber",
                table: "ExternalOperations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations",
                column: "SenderId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
