﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApplicationServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Entity.Advertisement", b =>
                {
                    b.Property<int>("AdvertisementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("advertisement_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("AdvertisementId"));

                    b.Property<int>("Available")
                        .HasColumnType("int")
                        .HasColumnName("available");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("company_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedAt"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("description");

                    b.Property<int>("Duration")
                        .HasColumnType("int")
                        .HasColumnName("duration");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("name");

                    b.Property<bool>("Open")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("open");

                    b.Property<string>("Questionnaire")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("questionnaire");

                    b.Property<int>("Spots")
                        .HasColumnType("int")
                        .HasColumnName("spots");

                    b.HasKey("AdvertisementId");

                    b.HasIndex("CompanyId");

                    b.ToTable("advertisement");
                });

            modelBuilder.Entity("Entity.AdvertisementSkills", b =>
                {
                    b.Property<int>("AdvertisementId")
                        .HasColumnType("int")
                        .HasColumnName("advertisement_id")
                        .HasColumnOrder(0);

                    b.Property<int>("SkillId")
                        .HasColumnType("int")
                        .HasColumnName("skill_id")
                        .HasColumnOrder(1);

                    b.HasKey("AdvertisementId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("advertisement_skills");
                });

            modelBuilder.Entity("Entity.Application", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("application_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ApplicationId"));

                    b.Property<int>("AdvertisementId")
                        .HasColumnType("int")
                        .HasColumnName("advertisement_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedAt"));

                    b.Property<string>("Questionnaire")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("questionnaire");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("enum('PENDING', 'ACCEPTED', 'REJECTED')")
                        .HasColumnName("status");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.HasKey("ApplicationId");

                    b.HasIndex("AdvertisementId");

                    b.HasIndex("StudentId");

                    b.ToTable("application");
                });

            modelBuilder.Entity("Entity.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("company_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Bio")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("bio");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedAt"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("email");

                    b.Property<string>("FiscalCode")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("fiscal_code");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(44)
                        .HasColumnType("varchar(44)")
                        .HasColumnName("hashed_password");

                    b.Property<string>("Headquarter")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("headquarter");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)")
                        .HasColumnName("salt");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("username");

                    b.Property<string>("VatNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("vat_number");

                    b.HasKey("CompanyId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("company");
                });

            modelBuilder.Entity("Entity.CompanyFeedback", b =>
                {
                    b.Property<int>("InternshipId")
                        .HasColumnType("int")
                        .HasColumnName("internship_id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("comment");

                    b.Property<int>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.HasKey("InternshipId");

                    b.ToTable("company_feedback");
                });

            modelBuilder.Entity("Entity.Internship", b =>
                {
                    b.Property<int>("InternshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("internship_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("InternshipId"));

                    b.Property<int>("AdvertisementId")
                        .HasColumnType("int")
                        .HasColumnName("advertisement_id");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("company_id");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedAt"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("end_date");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("start_date");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.HasKey("InternshipId");

                    b.HasIndex("AdvertisementId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("StudentId");

                    b.ToTable("internship");
                });

            modelBuilder.Entity("Entity.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("skill_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("SkillId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("name");

                    b.HasKey("SkillId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("skill");
                });

            modelBuilder.Entity("Entity.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<string>("Bio")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("bio");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("birth_date");

                    b.Property<string>("CourseOfStudy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("course_of_study");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    MySqlPropertyBuilderExtensions.UseMySqlComputedColumn(b.Property<DateTime>("CreatedAt"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("email");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("enum('m','f')")
                        .HasColumnName("gender");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(44)
                        .HasColumnType("varchar(44)")
                        .HasColumnName("hashed_password");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("name");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)")
                        .HasColumnName("salt");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("surname");

                    b.Property<string>("University")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("university");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)")
                        .HasColumnName("username");

                    b.HasKey("StudentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("student");
                });

            modelBuilder.Entity("Entity.StudentFeedback", b =>
                {
                    b.Property<int>("InternshipId")
                        .HasColumnType("int")
                        .HasColumnName("internship_id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("comment");

                    b.Property<int>("Rating")
                        .HasColumnType("int")
                        .HasColumnName("rating");

                    b.HasKey("InternshipId");

                    b.ToTable("student_feedback");
                });

            modelBuilder.Entity("Entity.StudentNotifications", b =>
                {
                    b.Property<int>("StudentNotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("student_notification_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("StudentNotificationId"));

                    b.Property<int>("AdvertisementId")
                        .HasColumnType("int")
                        .HasColumnName("advertisement_id");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("enum('INVITED', 'RECOMMENDED', 'ACCEPTED', 'REJECTED')")
                        .HasColumnName("type");

                    b.HasKey("StudentNotificationId");

                    b.HasIndex("AdvertisementId");

                    b.HasIndex("StudentId");

                    b.ToTable("student_notifications");
                });

            modelBuilder.Entity("Entity.StudentSkills", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("student_id")
                        .HasColumnOrder(0);

                    b.Property<int>("SkillId")
                        .HasColumnType("int")
                        .HasColumnName("skill_id")
                        .HasColumnOrder(1);

                    b.HasKey("StudentId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("student_skills");
                });

            modelBuilder.Entity("Entity.Advertisement", b =>
                {
                    b.HasOne("Entity.Company", "Company")
                        .WithMany("Advertisements")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Entity.AdvertisementSkills", b =>
                {
                    b.HasOne("Entity.Advertisement", "Advertisement")
                        .WithMany("AdvertisementSkills")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Skill", "Skill")
                        .WithMany("AdvertisementSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advertisement");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("Entity.Application", b =>
                {
                    b.HasOne("Entity.Advertisement", "Advertisement")
                        .WithMany("Applications")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Student", "Student")
                        .WithMany("Applications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advertisement");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Entity.CompanyFeedback", b =>
                {
                    b.HasOne("Entity.Internship", "Internship")
                        .WithOne("CompanyFeedback")
                        .HasForeignKey("Entity.CompanyFeedback", "InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Internship");
                });

            modelBuilder.Entity("Entity.Internship", b =>
                {
                    b.HasOne("Entity.Advertisement", "Advertisement")
                        .WithMany("Internships")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Company", "Company")
                        .WithMany("Internships")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Student", "Student")
                        .WithMany("Internships")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advertisement");

                    b.Navigation("Company");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Entity.StudentFeedback", b =>
                {
                    b.HasOne("Entity.Internship", "Internship")
                        .WithOne("StudentFeedback")
                        .HasForeignKey("Entity.StudentFeedback", "InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Internship");
                });

            modelBuilder.Entity("Entity.StudentNotifications", b =>
                {
                    b.HasOne("Entity.Advertisement", "Advertisement")
                        .WithMany("StudentNotifications")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Student", "Student")
                        .WithMany("StudentNotifications")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advertisement");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Entity.StudentSkills", b =>
                {
                    b.HasOne("Entity.Skill", "Skill")
                        .WithMany("StudentSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entity.Student", "Student")
                        .WithMany("StudentSkills")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Entity.Advertisement", b =>
                {
                    b.Navigation("AdvertisementSkills");

                    b.Navigation("Applications");

                    b.Navigation("Internships");

                    b.Navigation("StudentNotifications");
                });

            modelBuilder.Entity("Entity.Company", b =>
                {
                    b.Navigation("Advertisements");

                    b.Navigation("Internships");
                });

            modelBuilder.Entity("Entity.Internship", b =>
                {
                    b.Navigation("CompanyFeedback");

                    b.Navigation("StudentFeedback");
                });

            modelBuilder.Entity("Entity.Skill", b =>
                {
                    b.Navigation("AdvertisementSkills");

                    b.Navigation("StudentSkills");
                });

            modelBuilder.Entity("Entity.Student", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("Internships");

                    b.Navigation("StudentNotifications");

                    b.Navigation("StudentSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
