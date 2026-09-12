using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Access_Authority.Migrations
{
    /// <inheritdoc />
    public partial class AddCareerPositionToApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CareerPositionId",
                table: "CareerViewModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareerViewModels_CareerPositionId",
                table: "CareerViewModels",
                column: "CareerPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareerViewModels_CareerPositions_CareerPositionId",
                table: "CareerViewModels",
                column: "CareerPositionId",
                principalTable: "CareerPositions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareerViewModels_CareerPositions_CareerPositionId",
                table: "CareerViewModels");

            migrationBuilder.DropIndex(
                name: "IX_CareerViewModels_CareerPositionId",
                table: "CareerViewModels");

            migrationBuilder.DropColumn(
                name: "CareerPositionId",
                table: "CareerViewModels");
        }
    }
}
