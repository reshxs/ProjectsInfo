using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ProjectsInfo.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Salary = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ExpectedHours = table.Column<int>(nullable: false),
                    DevelopmentHourPrice = table.Column<decimal>(nullable: false),
                    TestingHours = table.Column<int>(nullable: false),
                    TestingHourPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsManager = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAssigments",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false),
                    DeveloperID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAssigments", x => new { x.ProjectID, x.DeveloperID });
                    table.ForeignKey(
                        name: "FK_ProjectAssigments_Developers_DeveloperID",
                        column: x => x.DeveloperID,
                        principalTable: "Developers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectAssigments_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Months",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Hours = table.Column<int>(nullable: false),
                    DeveloperAssignmentProjectID = table.Column<int>(nullable: false),
                    DeveloperAssignmentDeveloperID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Months", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Months_ProjectAssigments_DeveloperAssignmentProjectID_Devel~",
                        columns: x => new { x.DeveloperAssignmentProjectID, x.DeveloperAssignmentDeveloperID },
                        principalTable: "ProjectAssigments",
                        principalColumns: new[] { "ProjectID", "DeveloperID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Months_DeveloperAssignmentProjectID_DeveloperAssignmentDeve~",
                table: "Months",
                columns: new[] { "DeveloperAssignmentProjectID", "DeveloperAssignmentDeveloperID" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssigments_DeveloperID",
                table: "ProjectAssigments",
                column: "DeveloperID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Months");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProjectAssigments");

            migrationBuilder.DropTable(
                name: "Developers");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
