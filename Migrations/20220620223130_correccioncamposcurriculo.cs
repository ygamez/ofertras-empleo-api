using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations
{
    public partial class correccioncamposcurriculo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "correo",
                table: "Curriculos",
                newName: "Correo");

            migrationBuilder.RenameColumn(
                name: "ExperienciaLaboral",
                table: "Curriculos",
                newName: "Experiencialaboral");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Experiencialaboral",
                table: "Curriculos",
                newName: "ExperienciaLaboral");

            migrationBuilder.RenameColumn(
                name: "Correo",
                table: "Curriculos",
                newName: "correo");
        }
    }
}
