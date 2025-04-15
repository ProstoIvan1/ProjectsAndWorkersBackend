using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectsAndWorkers.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Worker_AuthorId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Worker_PerformerId",
                table: "Task");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Worker_AuthorId",
                table: "Task",
                column: "AuthorId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Worker_PerformerId",
                table: "Task",
                column: "PerformerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Worker_AuthorId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Worker_PerformerId",
                table: "Task");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Worker_AuthorId",
                table: "Task",
                column: "AuthorId",
                principalTable: "Worker",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Worker_PerformerId",
                table: "Task",
                column: "PerformerId",
                principalTable: "Worker",
                principalColumn: "Id");
        }
    }
}
