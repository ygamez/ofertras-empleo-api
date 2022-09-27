using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations
{
    public partial class arreglocurtele : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Telefono",
                table: "Curriculos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Curriculos");
        }
    }
}
