using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLendingSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: false),
                    RoleType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    NPM = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyProgram = table.Column<byte>(type: "tinyint", nullable: false),
                    UserAccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => new { x.Id, x.NPM });
                    table.ForeignKey(
                        name: "FK_Student_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_UserAccountId",
                table: "Role",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserAccountId",
                table: "Student",
                column: "UserAccountId",
                unique: true,
                filter: "[UserAccountId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "UserAccount");
        }
    }
}
