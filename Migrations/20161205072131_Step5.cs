using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyseVisitorsTool.Migrations
{
    public partial class Step5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleMapsAPIKey",
                table: "Setup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPLocationAPIKey",
                table: "Setup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleMapsAPIKey",
                table: "Setup");

            migrationBuilder.DropColumn(
                name: "IPLocationAPIKey",
                table: "Setup");
        }
    }
}
