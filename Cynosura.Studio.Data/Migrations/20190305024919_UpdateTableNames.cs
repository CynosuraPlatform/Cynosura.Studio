using Microsoft.EntityFrameworkCore.Migrations;

namespace Cynosura.Studio.Data.Migrations
{
    public partial class UpdateTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solution_AspNetUsers_CreationUserId",
                table: "Solution");

            migrationBuilder.DropForeignKey(
                name: "FK_Solution_AspNetUsers_ModificationUserId",
                table: "Solution");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solution",
                table: "Solution");

            migrationBuilder.RenameTable(
                name: "Solution",
                newName: "Solutions");

            migrationBuilder.RenameIndex(
                name: "IX_Solution_ModificationUserId",
                table: "Solutions",
                newName: "IX_Solutions_ModificationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Solution_CreationUserId",
                table: "Solutions",
                newName: "IX_Solutions_CreationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solutions",
                table: "Solutions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_AspNetUsers_CreationUserId",
                table: "Solutions",
                column: "CreationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_AspNetUsers_ModificationUserId",
                table: "Solutions",
                column: "ModificationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_AspNetUsers_CreationUserId",
                table: "Solutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_AspNetUsers_ModificationUserId",
                table: "Solutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solutions",
                table: "Solutions");

            migrationBuilder.RenameTable(
                name: "Solutions",
                newName: "Solution");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_ModificationUserId",
                table: "Solution",
                newName: "IX_Solution_ModificationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Solutions_CreationUserId",
                table: "Solution",
                newName: "IX_Solution_CreationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solution",
                table: "Solution",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solution_AspNetUsers_CreationUserId",
                table: "Solution",
                column: "CreationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Solution_AspNetUsers_ModificationUserId",
                table: "Solution",
                column: "ModificationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
