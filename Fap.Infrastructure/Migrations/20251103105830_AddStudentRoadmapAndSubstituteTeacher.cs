using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fap.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentRoadmapAndSubstituteTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Universities_UniversityId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Universities_UniversityId",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Universities_UniversityId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "StudentTranscripts");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropIndex(
                name: "IX_Users_UniversityId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_UniversityId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Classes_UniversityId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Classes");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinScore",
                table: "SubjectCriteria",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPA",
                table: "Students",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "GraduationDate",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGraduated",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Slots",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Slots",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Slots",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "SubstituteTeacherId",
                table: "Slots",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubstitutionReason",
                table: "Slots",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Slots",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "Grades",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "StudentRoadmaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SemesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false),
                    FinalScore = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    LetterGrade = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRoadmaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRoadmaps_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentRoadmaps_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentRoadmaps_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Slots_SubstituteTeacherId",
                table: "Slots",
                column: "SubstituteTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRoadmaps_SemesterId",
                table: "StudentRoadmaps",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRoadmaps_StudentId",
                table: "StudentRoadmaps",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRoadmaps_SubjectId",
                table: "StudentRoadmaps",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Teachers_SubstituteTeacherId",
                table: "Slots",
                column: "SubstituteTeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Teachers_SubstituteTeacherId",
                table: "Slots");

            migrationBuilder.DropTable(
                name: "StudentRoadmaps");

            migrationBuilder.DropIndex(
                name: "IX_Slots_SubstituteTeacherId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "GraduationDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsGraduated",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "SubstituteTeacherId",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "SubstitutionReason",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Slots");

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "Subjects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinScore",
                table: "SubjectCriteria",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "GPA",
                table: "Students",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "Grades",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentTranscripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicPhase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    FinalLetter = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    FinalScore = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    IsGraduated = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTranscripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTranscripts_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTranscripts_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UniversityId",
                table: "Users",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_UniversityId",
                table: "Subjects",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_UniversityId",
                table: "Classes",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTranscripts_StudentId",
                table: "StudentTranscripts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTranscripts_SubjectId",
                table: "StudentTranscripts",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Universities_UniversityId",
                table: "Classes",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Universities_UniversityId",
                table: "Subjects",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Universities_UniversityId",
                table: "Users",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id");
        }
    }
}
