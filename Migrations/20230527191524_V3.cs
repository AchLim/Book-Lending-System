using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Lending_System.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserPartner",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPartner_UserId",
                table: "UserPartner",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPartner_AspNetUsers_UserId",
                table: "UserPartner",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPartner_AspNetUsers_UserId",
                table: "UserPartner");

            migrationBuilder.DropIndex(
                name: "IX_UserPartner_UserId",
                table: "UserPartner");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserPartner");
        }
    }
}
