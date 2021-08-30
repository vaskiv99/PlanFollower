﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using Planner.Domain.Enum;
using Planner.Persistent;

namespace Planner.Persistent.Migrations
{
    [DbContext(typeof(PlannerDbContext))]
    partial class PlannerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresEnum(null, "equal_type", new[] { "less_than_or_equal", "equal", "greater_than_or_equal" })
                .HasPostgresEnum(null, "planner_status", new[] { "pending_start", "in_progress", "postponed", "stopped", "completed" })
                .HasPostgresEnum(null, "time_period", new[] { "day", "week", "month", "year" })
                .HasPostgresEnum(null, "tracking_type", new[] { "percentage", "abstract_goal_value" })
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Planner.Domain.AggregatesModel.GoalAggregate.Entities.Goal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal?>("AbstractGoalValue")
                        .HasColumnType("numeric")
                        .HasColumnName("abstract_goal_value");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<EqualType>("EqualType")
                        .HasColumnType("equal_type")
                        .HasColumnName("equal_type");

                    b.Property<string>("Frequency")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("frequency");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<Guid>("PlannerId")
                        .HasColumnType("uuid")
                        .HasColumnName("planner_id");

                    b.Property<PlannerStatus>("Status")
                        .HasColumnType("planner_status")
                        .HasColumnName("status");

                    b.Property<TrackingType>("TrackingType")
                        .HasColumnType("tracking_type")
                        .HasColumnName("tracking_type");

                    b.HasKey("Id")
                        .HasName("pk_planner_goals");

                    b.HasIndex("PlannerId")
                        .HasDatabaseName("ix_planner_goals_planner_id");

                    b.ToTable("planner_goals");
                });

            modelBuilder.Entity("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.Planner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<PlannerStatus>("CurrentStatus")
                        .HasColumnType("planner_status")
                        .HasColumnName("current_status");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_planners");

                    b.ToTable("planners");
                });

            modelBuilder.Entity("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.PlannerStatusItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<Guid>("PlannerId")
                        .HasColumnType("uuid")
                        .HasColumnName("planner_id");

                    b.Property<PlannerStatus>("Status")
                        .HasColumnType("planner_status")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_planner_status_items");

                    b.HasIndex("PlannerId")
                        .HasDatabaseName("ix_planner_status_items_planner_id");

                    b.ToTable("planner_status_items");
                });

            modelBuilder.Entity("Planner.Domain.AggregatesModel.GoalAggregate.Entities.Goal", b =>
                {
                    b.HasOne("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.Planner", null)
                        .WithMany()
                        .HasForeignKey("PlannerId")
                        .HasConstraintName("fk_planner_goals_planners_planner_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Planner.Domain.ValueObjects.Duration", "Duration", b1 =>
                        {
                            b1.Property<Guid>("GoalId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<DateTime>("End")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("duration_end");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("duration_start");

                            b1.HasKey("GoalId")
                                .HasName("pk_planner_goals");

                            b1.ToTable("planner_goals");

                            b1.WithOwner()
                                .HasForeignKey("GoalId")
                                .HasConstraintName("fk_planner_goals_planner_goals_id");
                        });

                    b.Navigation("Duration");
                });

            modelBuilder.Entity("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.Planner", b =>
                {
                    b.OwnsOne("Planner.Domain.ValueObjects.Duration", "Duration", b1 =>
                        {
                            b1.Property<Guid>("PlannerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<DateTime>("End")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("duration_end");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("timestamp without time zone")
                                .HasColumnName("duration_start");

                            b1.HasKey("PlannerId")
                                .HasName("pk_planners");

                            b1.ToTable("planners");

                            b1.WithOwner()
                                .HasForeignKey("PlannerId")
                                .HasConstraintName("fk_planners_planners_id");
                        });

                    b.Navigation("Duration");
                });

            modelBuilder.Entity("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.PlannerStatusItem", b =>
                {
                    b.HasOne("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.Planner", null)
                        .WithMany("Items")
                        .HasForeignKey("PlannerId")
                        .HasConstraintName("fk_planner_status_items_planners_planner_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Planner.Domain.AggregatesModel.PlannerAggregate.Entities.Planner", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
