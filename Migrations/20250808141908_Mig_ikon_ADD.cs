using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigferKariyer.Migrations
{
    /// <inheritdoc />
    public partial class Mig_ikon_ADD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ikon",
                table: "AboutLists",
                type: "NVARCHAR(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Başlık",
                table: "AboutLists",
                type: "NVARCHAR(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Açıklama",
                table: "AboutLists",
                type: "NVARCHAR(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(2000)",
                oldMaxLength: 2000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ikon",
                table: "AboutLists",
                type: "VarChar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Başlık",
                table: "AboutLists",
                type: "VarChar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Açıklama",
                table: "AboutLists",
                type: "VarChar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(2000)",
                oldMaxLength: 2000);
        }
    }
}
