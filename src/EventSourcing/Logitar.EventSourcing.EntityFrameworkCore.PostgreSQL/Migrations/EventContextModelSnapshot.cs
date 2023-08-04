﻿// <auto-generated />
using System;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL.Migrations
{
    [DbContext(typeof(EventContext))]
    partial class EventContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Logitar.EventSourcing.EntityFrameworkCore.Relational.EventEntity", b =>
                {
                    b.Property<long>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("EventId"));

                    b.Property<string>("ActorId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("AggregateType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("DeleteAction")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("EventData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("EventId");

                    b.HasIndex("ActorId");

                    b.HasIndex("AggregateId");

                    b.HasIndex("EventType");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("OccurredOn");

                    b.HasIndex("Version");

                    b.HasIndex("AggregateType", "AggregateId");

                    b.ToTable("Events", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
