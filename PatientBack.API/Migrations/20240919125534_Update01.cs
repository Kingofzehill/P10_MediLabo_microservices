using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientBackAPI.Migrations
{
    /// <inheritdoc />
    public partial class Update01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Addresses",
                newName: "AddressContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressContent",
                table: "Addresses",
                newName: "Name");
        }
    }
}
