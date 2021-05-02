using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileApplication.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RideRequest",
                columns: table => new
                {
                    RideRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEco = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideRequest", x => x.RideRequestId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideRequest");
        }
    }
}
