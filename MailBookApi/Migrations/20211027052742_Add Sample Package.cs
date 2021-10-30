using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailBookApi.Migrations
{
    public partial class AddSamplePackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "Id", "ArrivedDate", "DeliverCompanyId", "PackageNumber" },
                values: new object[] { 1, new DateTime(2021, 10, 15, 12, 0, 1, 0, DateTimeKind.Unspecified), 2, "PKG000001" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
