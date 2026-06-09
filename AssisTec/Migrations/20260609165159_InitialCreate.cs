using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AssisTec.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(maxLength: 100, nullable: false),
                    cpf = table.Column<string>(maxLength: 14, nullable: false),
                    telefone = table.Column<string>(maxLength: 20, nullable: true),
                    datanasc = table.Column<DateTime>(nullable: true),
                    cep = table.Column<string>(maxLength: 9, nullable: false),
                    rua = table.Column<string>(maxLength: 100, nullable: false),
                    numero = table.Column<string>(maxLength: 10, nullable: false),
                    cidade = table.Column<string>(maxLength: 60, nullable: false),
                    estado = table.Column<string>(maxLength: 100, nullable: false),
                    bairro = table.Column<string>(maxLength: 60, nullable: false),
                    complemento = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "equipamentos",
                columns: table => new
                {
                    id_equipamento = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(maxLength: 150, nullable: false),
                    marca = table.Column<string>(maxLength: 50, nullable: false),
                    modelo = table.Column<string>(maxLength: 50, nullable: false),
                    numero_serie = table.Column<string>(maxLength: 50, nullable: false),
                    estado_entrada = table.Column<string>(maxLength: 50, nullable: false),
                    acessorios = table.Column<string>(maxLength: 150, nullable: true),
                    observacoes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipamentos", x => x.id_equipamento);
                });

            migrationBuilder.CreateTable(
                name: "forma_pagamento",
                columns: table => new
                {
                    id_forma_pagamento = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forma_pagamento", x => x.id_forma_pagamento);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(maxLength: 100, nullable: false),
                    cpf = table.Column<string>(maxLength: 14, nullable: false),
                    senha = table.Column<string>(maxLength: 255, nullable: false),
                    telefone = table.Column<string>(maxLength: 20, nullable: true),
                    nivel = table.Column<int>(nullable: false),
                    status = table.Column<string>(maxLength: 20, nullable: false),
                    cep = table.Column<string>(maxLength: 9, nullable: false),
                    rua = table.Column<string>(maxLength: 100, nullable: false),
                    numero = table.Column<string>(maxLength: 10, nullable: false),
                    cidade = table.Column<string>(maxLength: 60, nullable: false),
                    bairro = table.Column<string>(maxLength: 60, nullable: false),
                    estado = table.Column<string>(maxLength: 100, nullable: false),
                    complemento = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id_usuario);
                });

            migrationBuilder.CreateTable(
                name: "ordem_servico",
                columns: table => new
                {
                    id_os = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    id_tecnico = table.Column<int>(nullable: true),
                    id_cliente = table.Column<int>(nullable: true),
                    id_equipamento = table.Column<int>(nullable: true),
                    status = table.Column<string>(maxLength: 30, nullable: false),
                    data_abertura = table.Column<DateTime>(nullable: false),
                    data_atualizacao = table.Column<DateTime>(nullable: true),
                    data_fechamento = table.Column<DateTime>(nullable: true),
                    valor_mao_obra = table.Column<decimal>(nullable: false),
                    valor_pecas = table.Column<decimal>(nullable: false),
                    valor_total = table.Column<decimal>(nullable: false),
                    problema_relatado = table.Column<string>(maxLength: 500, nullable: true),
                    diagnostico = table.Column<string>(maxLength: 500, nullable: true),
                    observacoes = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordem_servico", x => x.id_os);
                    table.ForeignKey(
                        name: "FK_ordem_servico_clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ordem_servico_equipamentos_id_equipamento",
                        column: x => x.id_equipamento,
                        principalTable: "equipamentos",
                        principalColumn: "id_equipamento",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ordem_servico_usuarios_id_tecnico",
                        column: x => x.id_tecnico,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "contas_receber",
                columns: table => new
                {
                    id_conta_receber = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(maxLength: 100, nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", maxLength: 18, nullable: false),
                    data_emissao = table.Column<DateTime>(nullable: false),
                    data_pagamento = table.Column<DateTime>(nullable: true),
                    data_vencimento = table.Column<DateTime>(nullable: false),
                    status = table.Column<string>(maxLength: 30, nullable: false),
                    observacoes = table.Column<string>(maxLength: 100, nullable: true),
                    id_os_fk = table.Column<int>(nullable: true),
                    id_forma_pagamento_fk = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contas_receber", x => x.id_conta_receber);
                    table.ForeignKey(
                        name: "FK_contas_receber_forma_pagamento_id_forma_pagamento_fk",
                        column: x => x.id_forma_pagamento_fk,
                        principalTable: "forma_pagamento",
                        principalColumn: "id_forma_pagamento",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_contas_receber_ordem_servico_id_os_fk",
                        column: x => x.id_os_fk,
                        principalTable: "ordem_servico",
                        principalColumn: "id_os",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "forma_pagamento",
                columns: new[] { "id_forma_pagamento", "descricao" },
                values: new object[,]
                {
                    { 1, "---" },
                    { 2, "Pix" },
                    { 3, "Cartão de Crédito / Débito" },
                    { 4, "Dinheiro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_contas_receber_id_forma_pagamento_fk",
                table: "contas_receber",
                column: "id_forma_pagamento_fk");

            migrationBuilder.CreateIndex(
                name: "IX_contas_receber_id_os_fk",
                table: "contas_receber",
                column: "id_os_fk");

            migrationBuilder.CreateIndex(
                name: "IX_ordem_servico_id_cliente",
                table: "ordem_servico",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_ordem_servico_id_equipamento",
                table: "ordem_servico",
                column: "id_equipamento");

            migrationBuilder.CreateIndex(
                name: "IX_ordem_servico_id_tecnico",
                table: "ordem_servico",
                column: "id_tecnico");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contas_receber");

            migrationBuilder.DropTable(
                name: "forma_pagamento");

            migrationBuilder.DropTable(
                name: "ordem_servico");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "equipamentos");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
