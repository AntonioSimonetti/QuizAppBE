using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeQuestionIdNullableInOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                schema: "QuizAppSchema",
                table: "Options");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                schema: "QuizAppSchema",
                table: "Options",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                schema: "QuizAppSchema",
                table: "Options",
                column: "QuestionId",
                principalSchema: "QuizAppSchema",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                schema: "QuizAppSchema",
                table: "Options");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                schema: "QuizAppSchema",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                schema: "QuizAppSchema",
                table: "Options",
                column: "QuestionId",
                principalSchema: "QuizAppSchema",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
