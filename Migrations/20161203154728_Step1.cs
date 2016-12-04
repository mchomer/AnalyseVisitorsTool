using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnalyseVisitorsTool.Migrations
{
    public partial class Step1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerLogFiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ClientIP = table.Column<string>(nullable: true),
                    ClientUserAgent = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    ServerIP = table.Column<string>(nullable: true),
                    ServerPort = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SubStatus = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TimeTaken = table.Column<string>(nullable: true),
                    UriQuery = table.Column<string>(nullable: true),
                    UriStem = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Win32Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerLogFiles", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerLogFiles");
        }
    }
}
