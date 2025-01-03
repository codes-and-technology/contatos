﻿// <auto-generated />
using System;
using DataBase.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataBase.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241110150038_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConsultingEntitys.ContactEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("DATETIME")
                        .HasColumnName("DataCriacao");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)")
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(250)")
                        .HasColumnName("Nome");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("VARCHAR(9)")
                        .HasColumnName("NumeroTelefone");

                    b.Property<Guid?>("PhoneRegionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PhoneRegionId");

                    b.ToTable("Contato", (string)null);
                });

            modelBuilder.Entity("ConsultingEntitys.PhoneRegionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("DATETIME")
                        .HasColumnName("DataCriacao");

                    b.Property<short>("RegionNumber")
                        .HasColumnType("smallint")
                        .HasColumnName("CodigoArea");

                    b.HasKey("Id");

                    b.ToTable("TelefoneRegiao", (string)null);
                });

            modelBuilder.Entity("ConsultingEntitys.ContactEntity", b =>
                {
                    b.HasOne("ConsultingEntitys.PhoneRegionEntity", "PhoneRegion")
                        .WithMany("Contacts")
                        .HasForeignKey("PhoneRegionId")
                        .HasConstraintName("IdArea");

                    b.Navigation("PhoneRegion");
                });

            modelBuilder.Entity("ConsultingEntitys.PhoneRegionEntity", b =>
                {
                    b.Navigation("Contacts");
                });
#pragma warning restore 612, 618
        }
    }
}
