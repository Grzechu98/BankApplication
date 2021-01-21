using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApplication.Migrations
{
    public partial class ExternalOperationsPreparation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Operations",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ExternalOperations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    OperationDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Incoming = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    ExternalAccountNumber = table.Column<string>(nullable: true),
                    TargetInternalAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalOperations_BankAccounts_TargetInternalAccountId",
                        column: x => x.TargetInternalAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalOperations_TargetInternalAccountId",
                table: "ExternalOperations",
                column: "TargetInternalAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations",
                column: "SenderId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations");

            migrationBuilder.DropTable(
                name: "ExternalOperations");

            migrationBuilder.AlterColumn<int>(
                name: "SenderId",
                table: "Operations",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_BankAccounts_SenderId",
                table: "Operations",
                column: "SenderId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
