using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fap.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectIdToGradeComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add SubjectId column as nullable first
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "GradeComponents",
                type: "uniqueidentifier",
                nullable: true);

            // Step 2: Update existing GradeComponents with a valid SubjectId
            // Get the first subject's ID and assign it to all existing grade components
            migrationBuilder.Sql(@"
                UPDATE gc 
                SET gc.SubjectId = (SELECT TOP 1 Id FROM Subjects ORDER BY SubjectCode)
                FROM GradeComponents gc
                WHERE gc.SubjectId IS NULL
            ");

            // Step 3: Make SubjectId NOT NULL now that all rows have valid data
            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "GradeComponents",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            // Step 4: Create index
            migrationBuilder.CreateIndex(
                name: "IX_GradeComponents_SubjectId",
                table: "GradeComponents",
                column: "SubjectId");

            // Step 5: Add foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_GradeComponents_Subjects_SubjectId",
                table: "GradeComponents",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeComponents_Subjects_SubjectId",
                table: "GradeComponents");

            migrationBuilder.DropIndex(
                name: "IX_GradeComponents_SubjectId",
                table: "GradeComponents");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "GradeComponents");
        }
    }
}
