using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApplicationAPI.Migrations
{
    public partial class addCurriculo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Usuarios_UsuarioId",
                table: "RefreshToken");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "RefreshToken",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Curriculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NivelEscolar = table.Column<string>(type: "text", nullable: true),
                    Carrera = table.Column<string>(type: "text", nullable: true),
                    ActividadFundamental = table.Column<string>(type: "text", nullable: true),
                    ActividadSecundaria = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculos", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Usuarios_UsuarioId",
                table: "RefreshToken",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Usuarios_UsuarioId",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Curriculos");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "RefreshToken",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Usuarios_UsuarioId",
                table: "RefreshToken",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
