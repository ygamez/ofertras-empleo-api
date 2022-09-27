using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations
{
    public partial class correccionCurriculofinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActResumen",
                table: "Curriculos",
                newName: "correo");

            migrationBuilder.RenameColumn(
                name: "ActGeneral",
                table: "Curriculos",
                newName: "Nombre");

            migrationBuilder.AddColumn<string>(
                name: "Educacion",
                table: "Curriculos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienciaLaboral",
                table: "Curriculos",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Educacion",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "ExperienciaLaboral",
                table: "Curriculos");

            migrationBuilder.RenameColumn(
                name: "correo",
                table: "Curriculos",
                newName: "ActResumen");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Curriculos",
                newName: "ActGeneral");
        }
    }
}
