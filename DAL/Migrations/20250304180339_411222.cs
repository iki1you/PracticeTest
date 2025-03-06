using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _411222 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TraineeCount",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TraineeCount",
                table: "Directions");

            migrationBuilder.CreateIndex(
                name: "IX_Trainees_Phone",
                table: "Trainees",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainees_Phone",
                table: "Trainees");

            migrationBuilder.AddColumn<int>(
                name: "TraineeCount",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TraineeCount",
                table: "Directions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
