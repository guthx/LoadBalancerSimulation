﻿// <auto-generated />
using System;
using LoadBalancerAPI.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LoadBalancerAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20191127102857_DatabaseFixed")]
    partial class DatabaseFixed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LoadBalancerAPI.Data.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("LoadBalancerAPI.Data.Models.Response", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("RequestId");

                    b.Property<int?>("RequestId1");

                    b.Property<long>("ServerId");

                    b.HasKey("Id");

                    b.HasIndex("RequestId1");

                    b.HasIndex("ServerId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("LoadBalancerAPI.Data.Models.Server", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int>("RequestLimit");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("LoadBalancerAPI.Data.Models.Response", b =>
                {
                    b.HasOne("LoadBalancerAPI.Data.Models.Request", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId1");

                    b.HasOne("LoadBalancerAPI.Data.Models.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
