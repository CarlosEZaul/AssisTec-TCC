using Microsoft.EntityFrameworkCore.Migrations;

namespace AssisTec.Migrations
{
    public partial class RenomearChavesSecundarias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_historico_alteracao_os_ordem_servico_id_os",
                table: "historico_alteracao_os");

            migrationBuilder.DropForeignKey(
                name: "FK_historico_alteracao_os_usuarios_id_usuario",
                table: "historico_alteracao_os");

            migrationBuilder.DropForeignKey(
                name: "FK_item_os_ordem_servico_id_OS",
                table: "item_os");

            migrationBuilder.DropForeignKey(
                name: "FK_item_os_produto_id_produto",
                table: "item_os");

            migrationBuilder.DropForeignKey(
                name: "FK_ordem_servico_clientes_id_cliente",
                table: "ordem_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordem_servico_equipamentos_id_equipamento",
                table: "ordem_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordem_servico_usuarios_id_tecnico",
                table: "ordem_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_servico_os_ordem_servico_id_OS",
                table: "servico_os");

            migrationBuilder.RenameColumn(
                name: "id_OS",
                table: "servico_os",
                newName: "id_os_fk");

            migrationBuilder.RenameIndex(
                name: "IX_servico_os_id_OS",
                table: "servico_os",
                newName: "IX_servico_os_id_os_fk");

            migrationBuilder.RenameColumn(
                name: "id_tecnico",
                table: "ordem_servico",
                newName: "id_tecnico_fk");

            migrationBuilder.RenameColumn(
                name: "id_equipamento",
                table: "ordem_servico",
                newName: "id_equipamento_fk");

            migrationBuilder.RenameColumn(
                name: "id_cliente",
                table: "ordem_servico",
                newName: "id_cliente_fk");

            migrationBuilder.RenameIndex(
                name: "IX_ordem_servico_id_tecnico",
                table: "ordem_servico",
                newName: "IX_ordem_servico_id_tecnico_fk");

            migrationBuilder.RenameIndex(
                name: "IX_ordem_servico_id_equipamento",
                table: "ordem_servico",
                newName: "IX_ordem_servico_id_equipamento_fk");

            migrationBuilder.RenameIndex(
                name: "IX_ordem_servico_id_cliente",
                table: "ordem_servico",
                newName: "IX_ordem_servico_id_cliente_fk");

            migrationBuilder.RenameColumn(
                name: "id_produto",
                table: "item_os",
                newName: "id_produto_fk");

            migrationBuilder.RenameColumn(
                name: "id_OS",
                table: "item_os",
                newName: "id_os_fk");

            migrationBuilder.RenameIndex(
                name: "IX_item_os_id_produto",
                table: "item_os",
                newName: "IX_item_os_id_produto_fk");

            migrationBuilder.RenameIndex(
                name: "IX_item_os_id_OS",
                table: "item_os",
                newName: "IX_item_os_id_os_fk");

            migrationBuilder.RenameColumn(
                name: "id_usuario",
                table: "historico_alteracao_os",
                newName: "id_usuario_fk");

            migrationBuilder.RenameColumn(
                name: "id_os",
                table: "historico_alteracao_os",
                newName: "id_os_fk");

            migrationBuilder.RenameIndex(
                name: "IX_historico_alteracao_os_id_usuario",
                table: "historico_alteracao_os",
                newName: "IX_historico_alteracao_os_id_usuario_fk");

            migrationBuilder.RenameIndex(
                name: "IX_historico_alteracao_os_id_os",
                table: "historico_alteracao_os",
                newName: "IX_historico_alteracao_os_id_os_fk");

            migrationBuilder.AddForeignKey(
                name: "FK_historico_alteracao_os_ordem_servico_id_os_fk",
                table: "historico_alteracao_os",
                column: "id_os_fk",
                principalTable: "ordem_servico",
                principalColumn: "id_os",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_historico_alteracao_os_usuarios_id_usuario_fk",
                table: "historico_alteracao_os",
                column: "id_usuario_fk",
                principalTable: "usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_item_os_ordem_servico_id_os_fk",
                table: "item_os",
                column: "id_os_fk",
                principalTable: "ordem_servico",
                principalColumn: "id_os",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_item_os_produto_id_produto_fk",
                table: "item_os",
                column: "id_produto_fk",
                principalTable: "produto",
                principalColumn: "id_produto",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordem_servico_clientes_id_cliente_fk",
                table: "ordem_servico",
                column: "id_cliente_fk",
                principalTable: "clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordem_servico_equipamentos_id_equipamento_fk",
                table: "ordem_servico",
                column: "id_equipamento_fk",
                principalTable: "equipamentos",
                principalColumn: "id_equipamento",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordem_servico_usuarios_id_tecnico_fk",
                table: "ordem_servico",
                column: "id_tecnico_fk",
                principalTable: "usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_servico_os_ordem_servico_id_os_fk",
                table: "servico_os",
                column: "id_os_fk",
                principalTable: "ordem_servico",
                principalColumn: "id_os",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_historico_alteracao_os_ordem_servico_id_os_fk",
                table: "historico_alteracao_os");

            migrationBuilder.DropForeignKey(
                name: "FK_historico_alteracao_os_usuarios_id_usuario_fk",
                table: "historico_alteracao_os");

            migrationBuilder.DropForeignKey(
                name: "FK_item_os_ordem_servico_id_os_fk",
                table: "item_os");

            migrationBuilder.DropForeignKey(
                name: "FK_item_os_produto_id_produto_fk",
                table: "item_os");

            migrationBuilder.DropForeignKey(
                name: "FK_ordem_servico_clientes_id_cliente_fk",
                table: "ordem_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordem_servico_equipamentos_id_equipamento_fk",
                table: "ordem_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_ordem_servico_usuarios_id_tecnico_fk",
                table: "ordem_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_servico_os_ordem_servico_id_os_fk",
                table: "servico_os");

            migrationBuilder.RenameColumn(
                name: "id_os_fk",
                table: "servico_os",
                newName: "id_OS");

            migrationBuilder.RenameIndex(
                name: "IX_servico_os_id_os_fk",
                table: "servico_os",
                newName: "IX_servico_os_id_OS");

            migrationBuilder.RenameColumn(
                name: "id_tecnico_fk",
                table: "ordem_servico",
                newName: "id_tecnico");

            migrationBuilder.RenameColumn(
                name: "id_equipamento_fk",
                table: "ordem_servico",
                newName: "id_equipamento");

            migrationBuilder.RenameColumn(
                name: "id_cliente_fk",
                table: "ordem_servico",
                newName: "id_cliente");

            migrationBuilder.RenameIndex(
                name: "IX_ordem_servico_id_tecnico_fk",
                table: "ordem_servico",
                newName: "IX_ordem_servico_id_tecnico");

            migrationBuilder.RenameIndex(
                name: "IX_ordem_servico_id_equipamento_fk",
                table: "ordem_servico",
                newName: "IX_ordem_servico_id_equipamento");

            migrationBuilder.RenameIndex(
                name: "IX_ordem_servico_id_cliente_fk",
                table: "ordem_servico",
                newName: "IX_ordem_servico_id_cliente");

            migrationBuilder.RenameColumn(
                name: "id_produto_fk",
                table: "item_os",
                newName: "id_produto");

            migrationBuilder.RenameColumn(
                name: "id_os_fk",
                table: "item_os",
                newName: "id_OS");

            migrationBuilder.RenameIndex(
                name: "IX_item_os_id_produto_fk",
                table: "item_os",
                newName: "IX_item_os_id_produto");

            migrationBuilder.RenameIndex(
                name: "IX_item_os_id_os_fk",
                table: "item_os",
                newName: "IX_item_os_id_OS");

            migrationBuilder.RenameColumn(
                name: "id_usuario_fk",
                table: "historico_alteracao_os",
                newName: "id_usuario");

            migrationBuilder.RenameColumn(
                name: "id_os_fk",
                table: "historico_alteracao_os",
                newName: "id_os");

            migrationBuilder.RenameIndex(
                name: "IX_historico_alteracao_os_id_usuario_fk",
                table: "historico_alteracao_os",
                newName: "IX_historico_alteracao_os_id_usuario");

            migrationBuilder.RenameIndex(
                name: "IX_historico_alteracao_os_id_os_fk",
                table: "historico_alteracao_os",
                newName: "IX_historico_alteracao_os_id_os");

            migrationBuilder.AddForeignKey(
                name: "FK_historico_alteracao_os_ordem_servico_id_os",
                table: "historico_alteracao_os",
                column: "id_os",
                principalTable: "ordem_servico",
                principalColumn: "id_os",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_historico_alteracao_os_usuarios_id_usuario",
                table: "historico_alteracao_os",
                column: "id_usuario",
                principalTable: "usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_item_os_ordem_servico_id_OS",
                table: "item_os",
                column: "id_OS",
                principalTable: "ordem_servico",
                principalColumn: "id_os",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_item_os_produto_id_produto",
                table: "item_os",
                column: "id_produto",
                principalTable: "produto",
                principalColumn: "id_produto",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordem_servico_clientes_id_cliente",
                table: "ordem_servico",
                column: "id_cliente",
                principalTable: "clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordem_servico_equipamentos_id_equipamento",
                table: "ordem_servico",
                column: "id_equipamento",
                principalTable: "equipamentos",
                principalColumn: "id_equipamento",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ordem_servico_usuarios_id_tecnico",
                table: "ordem_servico",
                column: "id_tecnico",
                principalTable: "usuarios",
                principalColumn: "id_usuario",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_servico_os_ordem_servico_id_OS",
                table: "servico_os",
                column: "id_OS",
                principalTable: "ordem_servico",
                principalColumn: "id_os",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
