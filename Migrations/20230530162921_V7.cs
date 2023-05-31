using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Lending_System.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_AccountKey",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "AccountKey",
                table: "Student",
                newName: "UserPartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_AccountKey",
                table: "Student",
                newName: "IX_Student_UserPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_UserPartner_UserPartnerId",
                table: "Student",
                column: "UserPartnerId",
                principalTable: "UserPartner",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_UserPartner_UserPartnerId",
                table: "Student");

            migrationBuilder.RenameColumn(
                name: "UserPartnerId",
                table: "Student",
                newName: "AccountKey");

            migrationBuilder.RenameIndex(
                name: "IX_Student_UserPartnerId",
                table: "Student",
                newName: "IX_Student_AccountKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_AccountKey",
                table: "Student",
                column: "AccountKey",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
