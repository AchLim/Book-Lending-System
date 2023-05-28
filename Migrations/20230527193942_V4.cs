using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Lending_System.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBook",
                table: "UserBook");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserBook",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBook",
                table: "UserBook",
                columns: new[] { "Id", "UserId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_UserId",
                table: "UserBook",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBook",
                table: "UserBook");

            migrationBuilder.DropIndex(
                name: "IX_UserBook_UserId",
                table: "UserBook");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBook",
                table: "UserBook",
                columns: new[] { "UserId", "BookId" });
        }
    }
}
