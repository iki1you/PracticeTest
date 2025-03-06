using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _412 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainee_Direction_DirectionId",
                table: "Trainee");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainee_Project_ProjectId",
                table: "Trainee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainee",
                table: "Trainee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Direction",
                table: "Direction");

            migrationBuilder.RenameTable(
                name: "Trainee",
                newName: "Trainees");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Direction",
                newName: "Directions");

            migrationBuilder.RenameIndex(
                name: "IX_Trainee_ProjectId",
                table: "Trainees",
                newName: "IX_Trainees_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Trainee_Email",
                table: "Trainees",
                newName: "IX_Trainees_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Trainee_DirectionId",
                table: "Trainees",
                newName: "IX_Trainees_DirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainees",
                table: "Trainees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Directions",
                table: "Directions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Directions_DirectionId",
                table: "Trainees",
                column: "DirectionId",
                principalTable: "Directions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainees_Projects_ProjectId",
                table: "Trainees",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Directions_DirectionId",
                table: "Trainees");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainees_Projects_ProjectId",
                table: "Trainees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainees",
                table: "Trainees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Directions",
                table: "Directions");

            migrationBuilder.RenameTable(
                name: "Trainees",
                newName: "Trainee");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameTable(
                name: "Directions",
                newName: "Direction");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_ProjectId",
                table: "Trainee",
                newName: "IX_Trainee_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_Email",
                table: "Trainee",
                newName: "IX_Trainee_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Trainees_DirectionId",
                table: "Trainee",
                newName: "IX_Trainee_DirectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainee",
                table: "Trainee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Direction",
                table: "Direction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainee_Direction_DirectionId",
                table: "Trainee",
                column: "DirectionId",
                principalTable: "Direction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainee_Project_ProjectId",
                table: "Trainee",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
