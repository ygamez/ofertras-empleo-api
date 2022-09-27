using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplicationAPI.Migrations
{
    public partial class addCurriculoIdUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Curriculos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Curriculos_UsuarioId",
                table: "Curriculos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curriculos_Usuarios_UsuarioId",
                table: "Curriculos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curriculos_Usuarios_UsuarioId",
                table: "Curriculos");

            migrationBuilder.DropIndex(
                name: "IX_Curriculos_UsuarioId",
                table: "Curriculos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Curriculos");
        }
    }
}
