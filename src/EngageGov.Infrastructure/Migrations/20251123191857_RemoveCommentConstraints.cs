using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngageGov.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCommentConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Citizens_CitizenId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Citizens_CitizenId",
                table: "Proposals");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Citizens_CitizenId",
                table: "Comments",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Citizens_CitizenId",
                table: "Proposals",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Citizens_CitizenId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Citizens_CitizenId",
                table: "Proposals");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Citizens_CitizenId",
                table: "Comments",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Citizens_CitizenId",
                table: "Proposals",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
