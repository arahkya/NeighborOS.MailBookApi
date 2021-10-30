using Microsoft.EntityFrameworkCore.Migrations;

namespace MailBookApi.Migrations
{
    public partial class SetConstraintsandPrimaryKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Packages_DeliverCompanyId",
                table: "Packages");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DeliverCompanyId_PackageNumber",
                table: "Packages",
                columns: new[] { "DeliverCompanyId", "PackageNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliverCompanies_Name",
                table: "DeliverCompanies",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Packages_DeliverCompanyId_PackageNumber",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_DeliverCompanies_Name",
                table: "DeliverCompanies");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DeliverCompanyId",
                table: "Packages",
                column: "DeliverCompanyId");
        }
    }
}
