using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nest.Data.Migrations
{
    public partial class Added_Vendor_Logo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Vendors");
        }
    }
}
