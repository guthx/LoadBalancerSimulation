using Microsoft.EntityFrameworkCore.Migrations;

namespace LoadBalancerAPI.Migrations
{
    public partial class ServerIdIsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Servers_ServerId",
                table: "Responses");

            migrationBuilder.AlterColumn<long>(
                name: "ServerId",
                table: "Responses",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Servers_ServerId",
                table: "Responses",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Servers_ServerId",
                table: "Responses");

            migrationBuilder.AlterColumn<long>(
                name: "ServerId",
                table: "Responses",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Servers_ServerId",
                table: "Responses",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
