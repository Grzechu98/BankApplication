using Microsoft.EntityFrameworkCore.Migrations;

namespace BankApplication.Migrations
{
    public partial class u1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PESEL_IdentityDocumentNumber_Email_PhoneNumber_Login",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PESEL",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PIN",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PIN_IdentityDocumentNumber_Email_PhoneNumber_Login",
                table: "Users",
                columns: new[] { "PIN", "IdentityDocumentNumber", "Email", "PhoneNumber", "Login" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PIN_IdentityDocumentNumber_Email_PhoneNumber_Login",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PIN",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Login",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "PESEL",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PESEL_IdentityDocumentNumber_Email_PhoneNumber_Login",
                table: "Users",
                columns: new[] { "PESEL", "IdentityDocumentNumber", "Email", "PhoneNumber", "Login" },
                unique: true,
                filter: "[Login] IS NOT NULL");
        }
    }
}
