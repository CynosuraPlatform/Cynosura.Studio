using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cynosura.Studio.Data.Migrations
{
    public partial class AddWorkers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkerInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ClassName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreationUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ModificationUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerInfos_AspNetUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerInfos_AspNetUsers_ModificationUserId",
                        column: x => x.ModificationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkerRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkerInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    Result = table.Column<string>(type: "TEXT", nullable: true),
                    ResultData = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreationUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ModificationUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerRuns_AspNetUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerRuns_AspNetUsers_ModificationUserId",
                        column: x => x.ModificationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkerRuns_WorkerInfos_WorkerInfoId",
                        column: x => x.WorkerInfoId,
                        principalTable: "WorkerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerInfos_CreationUserId",
                table: "WorkerInfos",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerInfos_ModificationUserId",
                table: "WorkerInfos",
                column: "ModificationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRuns_CreationUserId",
                table: "WorkerRuns",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRuns_ModificationUserId",
                table: "WorkerRuns",
                column: "ModificationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerRuns_WorkerInfoId",
                table: "WorkerRuns",
                column: "WorkerInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerRuns");

            migrationBuilder.DropTable(
                name: "WorkerInfos");
        }
    }
}
