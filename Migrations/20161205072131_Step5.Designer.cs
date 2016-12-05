using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AnalyseVisitorsTool.Data;

namespace AnalyseVisitorsTool.Migrations
{
    [DbContext(typeof(LogDbContext))]
    [Migration("20161205072131_Step5")]
    partial class Step5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("AnalyseVisitorsTool.Models.IPLocation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("cityName");

                    b.Property<string>("countryCode");

                    b.Property<string>("countryName");

                    b.Property<string>("ipAddress");

                    b.Property<string>("latitude");

                    b.Property<string>("longitude");

                    b.Property<string>("regionName");

                    b.Property<string>("statusCode");

                    b.Property<string>("statusMessage");

                    b.Property<string>("timeZone");

                    b.Property<string>("zipCode");

                    b.HasKey("ID");

                    b.ToTable("IPLocations");
                });

            modelBuilder.Entity("AnalyseVisitorsTool.Models.ServerLogFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("ClientIP");

                    b.Property<string>("ClientUserAgent");

                    b.Property<string>("Country");

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<string>("Method");

                    b.Property<string>("ServerIP");

                    b.Property<string>("ServerPort");

                    b.Property<string>("Status");

                    b.Property<string>("SubStatus");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<string>("TimeTaken");

                    b.Property<string>("UriQuery");

                    b.Property<string>("UriStem");

                    b.Property<string>("Username");

                    b.Property<string>("Win32Status");

                    b.HasKey("ID");

                    b.ToTable("ServerLogFiles");
                });

            modelBuilder.Entity("AnalyseVisitorsTool.Models.Settings", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("GoogleMapsAPIKey");

                    b.Property<string>("IPLocationAPIKey");

                    b.Property<string>("ServerLogFilesFolder");

                    b.HasKey("ID");

                    b.ToTable("Setup");
                });
        }
    }
}
