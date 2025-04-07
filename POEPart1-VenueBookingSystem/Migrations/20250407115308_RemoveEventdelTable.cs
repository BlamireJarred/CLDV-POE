using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POEPart1_VenueBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEventdelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EvenName",
                table: "Event",
                newName: "EventName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventName",
                table: "Event",
                newName: "EvenName");
        }
    }
}
