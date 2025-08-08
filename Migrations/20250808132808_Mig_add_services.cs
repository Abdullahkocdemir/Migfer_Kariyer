using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigferKariyer.Migrations
{
    /// <inheritdoc />
    public partial class Mig_add_services : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "İkon",
                table: "Services",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "İkon",
                table: "Services");
        }
    }
}
