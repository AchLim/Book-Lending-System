using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Lending_System.Migrations
{
    /// <inheritdoc />
    public partial class V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_UserPartner_UserId",
                table: "UserBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPartner",
                table: "UserPartner");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserPartner",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBook",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPartner",
                table: "UserPartner",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartner_NIK",
                table: "UserPartner",
                column: "NIK",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_UserPartner_UserId",
                table: "UserBook",
                column: "UserId",
                principalTable: "UserPartner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBook_UserPartner_UserId",
                table: "UserBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPartner",
                table: "UserPartner");

            migrationBuilder.DropIndex(
                name: "IX_UserPartner_NIK",
                table: "UserPartner");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserPartner");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserBook",
                type: "nvarchar(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPartner",
                table: "UserPartner",
                column: "NIK");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBook_UserPartner_UserId",
                table: "UserBook",
                column: "UserId",
                principalTable: "UserPartner",
                principalColumn: "NIK",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
