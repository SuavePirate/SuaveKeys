﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SuaveKeys.Infrastructure.Data.Contexts;

namespace SuaveKeys.Infrastructure.Data.Migrations
{
    [DbContext(typeof(SuaveKeysContext))]
    [Migration("20200715231031_AuthCodeNewProperties")]
    partial class AuthCodeNewProperties
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SuaveKeys.Core.Models.Entities.AuthClient", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthCodeRedirectUrlHost")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TokenRedirectUrlHost")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AuthClients");
                });

            modelBuilder.Entity("SuaveKeys.Core.Models.Entities.AuthorizationCode", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthClientId")
                        .HasColumnType("text");

                    b.Property<string>("ChallengeCode")
                        .HasColumnType("text");

                    b.Property<string>("ChallengeMethod")
                        .HasColumnType("text");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthClientId");

                    b.ToTable("AuthorizationCodes");
                });

            modelBuilder.Entity("SuaveKeys.Core.Models.Entities.RefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AuthClientId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasAlternateKey("Token");

                    b.HasIndex("AuthClientId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SuaveKeys.Core.Models.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SuaveKeys.Core.Models.Entities.AuthorizationCode", b =>
                {
                    b.HasOne("SuaveKeys.Core.Models.Entities.AuthClient", "AuthClient")
                        .WithMany()
                        .HasForeignKey("AuthClientId");
                });

            modelBuilder.Entity("SuaveKeys.Core.Models.Entities.RefreshToken", b =>
                {
                    b.HasOne("SuaveKeys.Core.Models.Entities.AuthClient", "AuthClient")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AuthClientId");

                    b.HasOne("SuaveKeys.Core.Models.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
