using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigferKariyer.Migrations
{
    /// <inheritdoc />
    public partial class Mig_ikon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "İkon",
                table: "ContactLists",
                type: "NVARCHAR(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Açıklama",
                table: "ContactLists",
                type: "NVARCHAR(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Adı",
                table: "ContactLists",
                type: "NVARCHAR(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VarChar(30)",
                oldMaxLength: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "İkon",
                table: "ContactLists",
                type: "VarChar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Açıklama",
                table: "ContactLists",
                type: "VarChar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Adı",
                table: "ContactLists",
                type: "VarChar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(30)",
                oldMaxLength: 30);
        }
    }
}
