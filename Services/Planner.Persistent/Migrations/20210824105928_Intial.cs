using Microsoft.EntityFrameworkCore.Migrations;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using Planner.Domain.Enum;
using System;

namespace Planner.Persistent.Migrations
{
    public partial class Intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:equal_type", "less_than_or_equal,equal,greater_than_or_equal")
                .Annotation("Npgsql:Enum:planner_status", "pending_start,in_progress,postponed,stopped,completed")
                .Annotation("Npgsql:Enum:time_period", "day,week,month,year")
                .Annotation("Npgsql:Enum:tracking_type", "percentage,abstract_goal_value");

            migrationBuilder.CreateTable(
                name: "planners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    duration_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    duration_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    current_status = table.Column<PlannerStatus>(type: "planner_status", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_planners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "planner_goals",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    duration_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    duration_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    frequency = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    planner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    abstract_goal_value = table.Column<decimal>(type: "numeric", nullable: true),
                    tracking_type = table.Column<TrackingType>(type: "tracking_type", nullable: false),
                    equal_type = table.Column<EqualType>(type: "equal_type", nullable: false),
                    status = table.Column<PlannerStatus>(type: "planner_status", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_planner_goals", x => x.id);
                    table.ForeignKey(
                        name: "fk_planner_goals_planners_planner_id",
                        column: x => x.planner_id,
                        principalTable: "planners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "planner_status_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<PlannerStatus>(type: "planner_status", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    planner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_planner_status_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_planner_status_items_planners_planner_id",
                        column: x => x.planner_id,
                        principalTable: "planners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_planner_goals_planner_id",
                table: "planner_goals",
                column: "planner_id");

            migrationBuilder.CreateIndex(
                name: "ix_planner_status_items_planner_id",
                table: "planner_status_items",
                column: "planner_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "planner_goals");

            migrationBuilder.DropTable(
                name: "planner_status_items");

            migrationBuilder.DropTable(
                name: "planners");
        }
    }
}
