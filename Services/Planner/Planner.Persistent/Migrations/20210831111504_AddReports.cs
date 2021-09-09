using Microsoft.EntityFrameworkCore.Migrations;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;
using System;

namespace Planner.Persistent.Migrations
{
    public partial class AddReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "goal_reports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    value_of_progress = table.Column<decimal>(type: "numeric", nullable: false),
                    tracking_type = table.Column<TrackingType>(type: "tracking_type", nullable: false),
                    goal_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_goal_reports", x => x.id);
                    table.ForeignKey(
                        name: "fk_goal_reports_planner_goals_goal_id",
                        column: x => x.goal_id,
                        principalTable: "planner_goals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_goal_reports_goal_id",
                table: "goal_reports",
                column: "goal_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "goal_reports");
        }
    }
}
