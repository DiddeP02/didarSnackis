using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnackisUppgift.Migrations
{
    /// <inheritdoc />
    public partial class postreportinpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport",
                column: "PostId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport",
                column: "PostId");
        }
    }
}
