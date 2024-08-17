using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EasyTourChoice.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.AreaId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    Altitude = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActivityType = table.Column<int>(type: "INTEGER", nullable: false),
                    StartingLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Duration = table.Column<float>(type: "REAL", nullable: true),
                    ApproachDuration = table.Column<float>(type: "REAL", nullable: true),
                    MetersOfElevation = table.Column<int>(type: "INTEGER", nullable: true),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: true),
                    Risk = table.Column<int>(type: "INTEGER", nullable: true),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tours_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "AreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tours_Locations_ActivityLocationId",
                        column: x => x.ActivityLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tours_Locations_StartingLocationId",
                        column: x => x.StartingLocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "AreaId", "Name" },
                values: new object[,]
                {
                    { 1, "Safiental" },
                    { 2, "St. Antoenien" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Altitude", "Latitude", "Longitude" },
                values: new object[,]
                {
                    { 1, 1300.0, 46.733224, 9.3354219999999994 },
                    { 2, 1410.0, 46.968062000000003, 9.8151139999999995 }
                });

            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "Id", "ActivityLocationId", "ActivityType", "ApproachDuration", "AreaId", "Difficulty", "Duration", "MetersOfElevation", "Name", "Risk", "ShortDescription", "StartingLocationId" },
                values: new object[,]
                {
                    { 1, 1, 11, null, 1, null, 5f, 1200, "FantasySkitourSafiental", 3, "Non existing ski tour for testing.", 1 },
                    { 2, 2, 11, null, 2, null, 5f, 1200, "FantasySkitourStAntoenien", 4, "Another non existing ski tour for testing.", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tours_ActivityLocationId",
                table: "Tours",
                column: "ActivityLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_AreaId",
                table: "Tours",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tours_StartingLocationId",
                table: "Tours",
                column: "StartingLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
