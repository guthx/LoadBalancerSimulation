using Microsoft.EntityFrameworkCore.Migrations;

namespace LoadBalancerAPI.Migrations
{
    public partial class DatabaseFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                table: "Responses",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "RequestId1",
                table: "Responses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RequestId1",
                table: "Responses",
                column: "RequestId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Requests_RequestId1",
                table: "Responses",
                column: "RequestId1",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Requests_RequestId1",
                table: "Responses");

            migrationBuilder.DropIndex(
                name: "IX_Responses_RequestId1",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "RequestId1",
                table: "Responses");
        }
    }
}
