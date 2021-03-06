﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AnalyseVisitorsTool.Data;

namespace AnalyseVisitorsTool.Migrations
{
    [DbContext(typeof(LogDbContext))]
    [Migration("20161203170323_Step2")]
    partial class Step2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("AnalyseVisitorsTool.Models.ServerLogFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientIP");

                    b.Property<string>("ClientUserAgent");

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
        }
    }
}
