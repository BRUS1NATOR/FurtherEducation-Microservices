using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.Infrastracture.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "profile_entity",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_entity", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "profile_student_diary_entity",
                columns: table => new
                {
                    course_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_name = table.Column<string>(type: "text", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_student_diary_entity", x => new { x.user_id, x.course_id });
                    table.ForeignKey(
                        name: "FK_profile_student_diary_entity_profile_entity_user_id",
                        column: x => x.user_id,
                        principalTable: "profile_entity",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_teacher_diary_entity",
                columns: table => new
                {
                    course_id = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_name = table.Column<string>(type: "text", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_teacher_diary_entity", x => new { x.user_id, x.course_id });
                    table.ForeignKey(
                        name: "FK_profile_teacher_diary_entity_profile_entity_user_id",
                        column: x => x.user_id,
                        principalTable: "profile_entity",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "profile_diary_recod_entity",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<string>(type: "text", nullable: false),
                    task_id = table.Column<string>(type: "text", nullable: false),
                    module_id = table.Column<string>(type: "text", nullable: false),
                    answer_given_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    score = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_diary_recod_entity", x => new { x.user_id, x.course_id, x.task_id });
                    table.ForeignKey(
                        name: "FK_profile_diary_recod_entity_profile_student_diary_entity_use~",
                        columns: x => new { x.user_id, x.course_id },
                        principalTable: "profile_student_diary_entity",
                        principalColumns: new[] { "user_id", "course_id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_diary_recod_entity");

            migrationBuilder.DropTable(
                name: "profile_teacher_diary_entity");

            migrationBuilder.DropTable(
                name: "profile_student_diary_entity");

            migrationBuilder.DropTable(
                name: "profile_entity");
        }
    }
}
