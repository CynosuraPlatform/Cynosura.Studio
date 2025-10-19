using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cynosura.Studio.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeNet8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PersistedGrants",
                table: "PersistedGrants");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "PersistedGrants",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PersistedGrants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersistedGrants",
                table: "PersistedGrants",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Use = table.Column<string>(type: "TEXT", nullable: true),
                    Algorithm = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsX509Certificate = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataProtected = table.Column<bool>(type: "INTEGER", nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PushedAuthorizationRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReferenceValueHash = table.Column<string>(type: "TEXT", nullable: true),
                    ExpiresAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Parameters = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushedAuthorizationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerSideSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Scheme = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SubjectId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Renewed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Expires = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Data = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerScheduleTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkerInfoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Seconds = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Minutes = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Hours = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DayOfMonth = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Month = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    DayOfWeek = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Year = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreationUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ModificationUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerScheduleTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerScheduleTasks_AspNetUsers_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerScheduleTasks_AspNetUsers_ModificationUserId",
                        column: x => x.ModificationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkerScheduleTasks_WorkerInfos_WorkerInfoId",
                        column: x => x.WorkerInfoId,
                        principalTable: "WorkerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants",
                column: "ConsumedTime");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Key",
                table: "PersistedGrants",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keys_Use",
                table: "Keys",
                column: "Use");

            migrationBuilder.CreateIndex(
                name: "IX_ServerSideSessions_DisplayName",
                table: "ServerSideSessions",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_ServerSideSessions_Expires",
                table: "ServerSideSessions",
                column: "Expires");

            migrationBuilder.CreateIndex(
                name: "IX_ServerSideSessions_Key",
                table: "ServerSideSessions",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerSideSessions_SessionId",
                table: "ServerSideSessions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerSideSessions_SubjectId",
                table: "ServerSideSessions",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerScheduleTasks_CreationUserId",
                table: "WorkerScheduleTasks",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerScheduleTasks_ModificationUserId",
                table: "WorkerScheduleTasks",
                column: "ModificationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerScheduleTasks_WorkerInfoId",
                table: "WorkerScheduleTasks",
                column: "WorkerInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "PushedAuthorizationRequests");

            migrationBuilder.DropTable(
                name: "ServerSideSessions");

            migrationBuilder.DropTable(
                name: "WorkerScheduleTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersistedGrants",
                table: "PersistedGrants");

            migrationBuilder.DropIndex(
                name: "IX_PersistedGrants_ConsumedTime",
                table: "PersistedGrants");

            migrationBuilder.DropIndex(
                name: "IX_PersistedGrants_Key",
                table: "PersistedGrants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PersistedGrants");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "PersistedGrants",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersistedGrants",
                table: "PersistedGrants",
                column: "Key");
        }
    }
}
