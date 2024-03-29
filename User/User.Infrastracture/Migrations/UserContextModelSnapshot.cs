﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using User.Infrastracture;

#nullable disable

namespace User.Infrastracture.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("en_US.utf8")
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("User.Domain.User.UserAttribute", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("id")
                        .HasDefaultValueSql("'sybase-needs-something-here'::character varying");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("user_id");

                    b.Property<string>("Value")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "UserId" }, "idx_user_attribute");

                    b.HasIndex(new[] { "Name", "Value" }, "idx_user_attribute_name");

                    b.ToTable("user_attribute", (string)null);
                });

            modelBuilder.Entity("User.Domain.User.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("id");

                    b.Property<long?>("CreatedTimestamp")
                        .HasColumnType("bigint")
                        .HasColumnName("created_timestamp");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("EmailConstraint")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email_constraint");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean")
                        .HasColumnName("email_verified");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<string>("FederationLink")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("federation_link");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("last_name");

                    b.Property<int>("NotBefore")
                        .HasColumnType("integer")
                        .HasColumnName("not_before");

                    b.Property<string>("RealmId")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("realm_id");

                    b.Property<string>("ServiceAccountClientLink")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("service_account_client_link");

                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Email" }, "idx_user_email");

                    b.HasIndex(new[] { "RealmId", "EmailConstraint" }, "uk_dykn684sl8up1crfei6eckhd7")
                        .IsUnique();

                    b.HasIndex(new[] { "RealmId", "Username" }, "uk_ru8tt6t700s9v50bu18ws5ha6")
                        .IsUnique();

                    b.ToTable("user_entity", (string)null);
                });

            modelBuilder.Entity("User.Domain.User.UserGroupMembership", b =>
                {
                    b.Property<string>("GroupId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("group_id");

                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("user_id");

                    b.HasKey("GroupId", "UserId")
                        .HasName("constraint_user_group");

                    b.HasIndex(new[] { "UserId" }, "idx_user_group_mapping");

                    b.ToTable("user_group_membership", (string)null);
                });

            modelBuilder.Entity("User.Domain.User.UsernameLoginFailure", b =>
                {
                    b.Property<string>("RealmId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("realm_id");

                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.Property<int?>("FailedLoginNotBefore")
                        .HasColumnType("integer")
                        .HasColumnName("failed_login_not_before");

                    b.Property<long?>("LastFailure")
                        .HasColumnType("bigint")
                        .HasColumnName("last_failure");

                    b.Property<string>("LastIpFailure")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("last_ip_failure");

                    b.Property<int?>("NumFailures")
                        .HasColumnType("integer")
                        .HasColumnName("num_failures");

                    b.HasKey("RealmId", "Username")
                        .HasName("CONSTRAINT_17-2");

                    b.ToTable("username_login_failure", (string)null);
                });

            modelBuilder.Entity("User.Domain.User.UserRoleMapping", b =>
                {
                    b.Property<string>("RoleId")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("role_id");

                    b.Property<string>("UserId")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("user_id");

                    b.HasKey("RoleId", "UserId")
                        .HasName("constraint_c");

                    b.HasIndex(new[] { "UserId" }, "idx_user_role_mapping");

                    b.ToTable("user_role_mapping", (string)null);
                });

            modelBuilder.Entity("User.Domain.UserProfile.UserProfileEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)")
                        .HasColumnName("id");

                    b.Property<List<string>>("LearningDiaries")
                        .HasColumnType("text[]")
                        .HasColumnName("learning_diaries");

                    b.Property<List<string>>("TeachingDiaries")
                        .HasColumnType("text[]")
                        .HasColumnName("teaching_diaries");

                    b.HasKey("Id");

                    b.ToTable("user_profile", (string)null);
                });

            modelBuilder.Entity("User.Domain.User.UserAttribute", b =>
                {
                    b.HasOne("User.Domain.User.UserEntity", "User")
                        .WithMany("UserAttributes")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("fk_5hrm2vlf9ql5fu043kqepovbr");

                    b.Navigation("User");
                });

            modelBuilder.Entity("User.Domain.User.UserGroupMembership", b =>
                {
                    b.HasOne("User.Domain.User.UserEntity", "User")
                        .WithMany("UserGroupMemberships")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("fk_user_group_user");

                    b.Navigation("User");
                });

            modelBuilder.Entity("User.Domain.User.UserRoleMapping", b =>
                {
                    b.HasOne("User.Domain.User.UserEntity", "User")
                        .WithMany("UserRoleMappings")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("fk_c4fqv34p1mbylloxang7b1q3l");

                    b.Navigation("User");
                });

            modelBuilder.Entity("User.Domain.UserProfile.UserProfileEntity", b =>
                {
                    b.HasOne("User.Domain.User.UserEntity", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("User.Domain.UserProfile.UserProfileEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("User.Domain.User.UserEntity", b =>
                {
                    b.Navigation("UserAttributes");

                    b.Navigation("UserGroupMemberships");

                    b.Navigation("UserProfile");

                    b.Navigation("UserRoleMappings");
                });
#pragma warning restore 612, 618
        }
    }
}
