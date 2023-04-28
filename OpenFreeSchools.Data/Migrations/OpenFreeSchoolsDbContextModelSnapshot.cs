﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenFreeSchools.Data;

#nullable disable

namespace OpenFreeSchools.Data.Migrations
{
    [DbContext(typeof(OpenFreeSchoolsDbContext))]
    partial class OpenFreeSchoolsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OpenFreeSchools.Data.Models.Audit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ChangeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValues")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("TimeOfChange")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__AuditLog");

                    b.ToTable("AuditLog", "concerns", t =>
                        {
                            t.HasTrigger("AuditLog_Trigger");
                        });
                });

            modelBuilder.Entity("OpenFreeSchools.Data.Models.Projects.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplicationWave")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projects", "openFreeSchool", t =>
                        {
                            t.HasTrigger("Projects_Trigger");
                        });
                });

            modelBuilder.Entity("OpenFreeSchools.Data.Models.SRMACase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CaseUrn")
                        .HasColumnType("int");

                    b.Property<int?>("CloseStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ClosedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateAccepted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOffered")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateReportSentToTrust")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDateOfVisit")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int?>("ReasonId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDateOfVisit")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReasonId");

                    b.ToTable("SRMACases", t =>
                        {
                            t.HasTrigger("SRMACases_Trigger");
                        });
                });

            modelBuilder.Entity("OpenFreeSchools.Data.Models.SRMAReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("SRMAReason", t =>
                        {
                            t.HasTrigger("SRMAReason_Trigger");
                        });
                });

            modelBuilder.Entity("OpenFreeSchools.Data.Models.SRMACase", b =>
                {
                    b.HasOne("OpenFreeSchools.Data.Models.SRMAReason", "Reason")
                        .WithMany()
                        .HasForeignKey("ReasonId");

                    b.Navigation("Reason");
                });
#pragma warning restore 612, 618
        }
    }
}
