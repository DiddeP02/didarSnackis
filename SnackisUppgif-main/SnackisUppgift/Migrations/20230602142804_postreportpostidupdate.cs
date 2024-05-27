using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnackisUppgift.Migrations
{
    /// <inheritdoc />
    public partial class postreportpostidupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReport_Post_PostId",
                table: "PostReport");

            migrationBuilder.DropIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostReport",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport",
                column: "PostId",
                unique: true,
                filter: "[PostId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReport_Post_PostId",
                table: "PostReport",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReport_Post_PostId",
                table: "PostReport");

            migrationBuilder.DropIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostReport",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostReport_PostId",
                table: "PostReport",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReport_Post_PostId",
                table: "PostReport",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
