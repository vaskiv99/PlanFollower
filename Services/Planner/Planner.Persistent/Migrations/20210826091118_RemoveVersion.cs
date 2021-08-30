using Microsoft.EntityFrameworkCore.Migrations;

namespace Planner.Persistent.Migrations
{
    public partial class RemoveVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "version",
                table: "planners");

            migrationBuilder.DropColumn(
                name: "version",
                table: "planner_status_items");

            migrationBuilder.DropColumn(
                name: "version",
                table: "planner_goals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "planners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "planner_status_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "planner_goals",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
