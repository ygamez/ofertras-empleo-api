using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations
{
    public partial class arreglocurr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActividadFundamental",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "ActividadSecundaria",
                table: "Curriculos");

            migrationBuilder.RenameColumn(
                name: "NivelEscolar",
                table: "Curriculos",
                newName: "ActResumen");

            migrationBuilder.RenameColumn(
                name: "Carrera",
                table: "Curriculos",
                newName: "ActGeneral");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActResumen",
                table: "Curriculos",
                newName: "NivelEscolar");

            migrationBuilder.RenameColumn(
                name: "ActGeneral",
                table: "Curriculos",
                newName: "Carrera");

            migrationBuilder.AddColumn<string>(
                name: "ActividadFundamental",
                table: "Curriculos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActividadSecundaria",
                table: "Curriculos",
                type: "text",
                nullable: true);
        }
    }
}
