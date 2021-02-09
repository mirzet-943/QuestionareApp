﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewsAPI.Models;

namespace NewsAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210209000821_candidates")]
    partial class candidates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("QuestionariesAppData.Candidate", b =>
                {
                    b.Property<int>("CandidateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CandidateId");

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("QuestionariesAppData.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("GradePattern")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuestionariesAppData.Models.QuestionResult", b =>
                {
                    b.Property<int>("QuestionResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Answer")
                        .HasColumnType("int");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<Guid>("QuestionareId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("QuestionResultId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("QuestionareId");

                    b.ToTable("QuestionResults");
                });

            modelBuilder.Entity("QuestionariesAppData.Models.Questionare", b =>
                {
                    b.Property<Guid>("QuestionareId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AccessTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FinishTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsSubmitted")
                        .HasColumnType("bit");

                    b.Property<string>("PinCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WrongPinsEntered")
                        .HasColumnType("int");

                    b.HasKey("QuestionareId");

                    b.HasIndex("UserId");

                    b.ToTable("Questionares");
                });

            modelBuilder.Entity("QuestionariesAppData.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QuestionariesAppData.Models.QuestionResult", b =>
                {
                    b.HasOne("QuestionariesAppData.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuestionariesAppData.Models.Questionare", "Questionare")
                        .WithMany()
                        .HasForeignKey("QuestionareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Questionare");
                });

            modelBuilder.Entity("QuestionariesAppData.Models.Questionare", b =>
                {
                    b.HasOne("QuestionariesAppData.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
