using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailBookApi.Migrations
{
    public partial class PackagesandDeliverCompanyInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliverCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ArrivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliverCompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_DeliverCompanies_DeliverCompanyId",
                        column: x => x.DeliverCompanyId,
                        principalTable: "DeliverCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DeliverCompanies",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "DHL" },
                    { 2, "SCG" },
                    { 3, "Flash" },
                    { 4, "Kerry" },
                    { 5, "Lex" },
                    { 6, "Shoppee" },
                    { 7, "Lazada" },
                    { 8, "Thai Post" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_DeliverCompanyId",
                table: "Packages",
                column: "DeliverCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "DeliverCompanies");
        }
    }
}
