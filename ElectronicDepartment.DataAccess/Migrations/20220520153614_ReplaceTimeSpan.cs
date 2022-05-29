using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicDepartment.DataAccess.Migrations
{
    public partial class ReplaceTimeSpan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpan",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LessonStart",
                table: "Lessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonStart",
                table: "Lessons");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpan",
                table: "Lessons",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
