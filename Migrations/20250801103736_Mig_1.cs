using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigferKariyer.Migrations
{
    /// <inheritdoc />
    public partial class Mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldofSpecializations",
                columns: table => new
                {
                    FieldofSpecializationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldofSpecializations", x => x.FieldofSpecializationId);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    InstructorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.InstructorId);
                });

            migrationBuilder.CreateTable(
                name: "InstructorFieldofSpecializations",
                columns: table => new
                {
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    FieldofSpecializationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorFieldofSpecializations", x => new { x.InstructorId, x.FieldofSpecializationId });
                    table.ForeignKey(
                        name: "FK_InstructorFieldofSpecializations_FieldofSpecializations_FieldofSpecializationId",
                        column: x => x.FieldofSpecializationId,
                        principalTable: "FieldofSpecializations",
                        principalColumn: "FieldofSpecializationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorFieldofSpecializations_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "InstructorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorFieldofSpecializations_FieldofSpecializationId",
                table: "InstructorFieldofSpecializations",
                column: "FieldofSpecializationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorFieldofSpecializations");

            migrationBuilder.DropTable(
                name: "FieldofSpecializations");

            migrationBuilder.DropTable(
                name: "Instructors");
        }
    }
}
