using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyseVisitorsTool.Migrations
{
    public partial class Step3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IPLocations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    cityName = table.Column<string>(nullable: true),
                    countryCode = table.Column<string>(nullable: true),
                    countryName = table.Column<string>(nullable: true),
                    ipAddress = table.Column<string>(nullable: true),
                    latitude = table.Column<string>(nullable: true),
                    longitude = table.Column<string>(nullable: true),
                    regionName = table.Column<string>(nullable: true),
                    statusCode = table.Column<string>(nullable: true),
                    statusMessage = table.Column<string>(nullable: true),
                    timeZone = table.Column<string>(nullable: true),
                    zipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPLocations", x => x.ID);
                });

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ServerLogFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ServerLogFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "ServerLogFiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "ServerLogFiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ServerLogFiles");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "ServerLogFiles");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ServerLogFiles");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ServerLogFiles");

            migrationBuilder.DropTable(
                name: "IPLocations");
        }
    }
}
