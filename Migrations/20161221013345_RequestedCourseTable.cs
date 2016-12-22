using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class RequestedCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_TeachingRequest_TeachingRequestSemesterID_TeachingRequestProfessorID",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_TeachingRequestSemesterID_TeachingRequestProfessorID",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TeachingRequestProfessorID",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TeachingRequestSemesterID",
                table: "Course");

            migrationBuilder.CreateTable(
                name: "RequestedCourse",
                columns: table => new
                {
                    CourseID = table.Column<int>(nullable: false),
                    RequestID = table.Column<int>(nullable: false),
                    Choice = table.Column<int>(nullable: false),
                    RequestProfessorID = table.Column<int>(nullable: true),
                    RequestSemesterID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedCourse", x => new { x.CourseID, x.RequestID });
                    table.ForeignKey(
                        name: "FK_RequestedCourse_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedCourse_TeachingRequest_RequestSemesterID_RequestProfessorID",
                        columns: x => new { x.RequestSemesterID, x.RequestProfessorID },
                        principalTable: "TeachingRequest",
                        principalColumns: new[] { "SemesterID", "ProfessorID" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestedCourse_CourseID",
                table: "RequestedCourse",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedCourse_RequestSemesterID_RequestProfessorID",
                table: "RequestedCourse",
                columns: new[] { "RequestSemesterID", "RequestProfessorID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestedCourse");

            migrationBuilder.AddColumn<int>(
                name: "TeachingRequestProfessorID",
                table: "Course",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeachingRequestSemesterID",
                table: "Course",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_TeachingRequestSemesterID_TeachingRequestProfessorID",
                table: "Course",
                columns: new[] { "TeachingRequestSemesterID", "TeachingRequestProfessorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Course_TeachingRequest_TeachingRequestSemesterID_TeachingRequestProfessorID",
                table: "Course",
                columns: new[] { "TeachingRequestSemesterID", "TeachingRequestProfessorID" },
                principalTable: "TeachingRequest",
                principalColumns: new[] { "SemesterID", "ProfessorID" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
