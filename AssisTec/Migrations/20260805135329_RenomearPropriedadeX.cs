using Microsoft.EntityFrameworkCore.Migrations;

namespace AssisTec.Migrations
{
    public partial class RenomearPropriedadeX : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "observacoes",
                table: "movimentacao_estoque");

            migrationBuilder.AddColumn<int>(
                name: "idUsuario",
                table: "movimentacao_estoque",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_movimentacao_estoque_idUsuario",
                table: "movimentacao_estoque",
                column: "idUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_movimentacao_estoque_usuarios_idUsuario",
                table: "movimentacao_estoque",
                column: "idUsuario",
                principalTable: "usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimentacao_estoque_usuarios_idUsuario",
                table: "movimentacao_estoque");

            migrationBuilder.DropIndex(
                name: "IX_movimentacao_estoque_idUsuario",
                table: "movimentacao_estoque");

            migrationBuilder.DropColumn(
                name: "idUsuario",
                table: "movimentacao_estoque");

            migrationBuilder.AddColumn<string>(
                name: "observacoes",
                table: "movimentacao_estoque",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
