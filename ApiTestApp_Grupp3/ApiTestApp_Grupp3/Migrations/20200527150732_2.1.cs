using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiTestApp_Grupp3.Migrations
{
    public partial class _21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeRole_EmployeeId",
                table: "EmployeeRole");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRole_RoleId",
                table: "EmployeeRole");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_RoleId",
                table: "EmployeeRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeRole_RoleId",
                table: "EmployeeRole");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_EmployeeId",
                table: "EmployeeRole",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_RoleId",
                table: "EmployeeRole",
                column: "RoleId",
                unique: true);
        }
    }
}
