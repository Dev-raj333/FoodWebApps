using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDonationWebApp.Migrations
{
    /// <inheritdoc />
    public partial class SecondDropRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DropRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonationID = table.Column<int>(type: "int", nullable: false),
                    VolunteerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PickupTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PickupStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DropRequests_AspNetUsers_VolunteerID",
                        column: x => x.VolunteerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DropRequests_Donations_DonationID",
                        column: x => x.DonationID,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DropRequests_DonationID",
                table: "DropRequests",
                column: "DonationID");

            migrationBuilder.CreateIndex(
                name: "IX_DropRequests_VolunteerID",
                table: "DropRequests",
                column: "VolunteerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropRequests");
        }
    }
}
