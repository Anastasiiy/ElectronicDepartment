using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicDepartment.DataAccess.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cafedras_CafedraId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cafedras_CafedraId",
                table: "AspNetUsers",
                column: "CafedraId",
                principalTable: "Cafedras",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cafedras_CafedraId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cafedras_CafedraId",
                table: "AspNetUsers",
                column: "CafedraId",
                principalTable: "Cafedras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
