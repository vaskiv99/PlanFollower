using Microsoft.EntityFrameworkCore.Migrations;
using Planner.Domain.Enum;
using System;

namespace Planner.Persistent.Migrations
{
    public partial class AddStatusHistoryToGoal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "planner_goals",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "goal_status_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<PlannerStatus>(type: "planner_status", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    goal_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_goal_status_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_goal_status_items_goals_goal_id",
                        column: x => x.goal_id,
                        principalTable: "planner_goals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_goal_status_items_goal_id",
                table: "goal_status_items",
                column: "goal_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "goal_status_items");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "planner_goals");
        }
    }
}
