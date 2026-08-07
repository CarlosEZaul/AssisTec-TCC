using Microsoft.EntityFrameworkCore.Migrations;

namespace AssisTec.Migrations
{
    public partial class fornecedor_em_produto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fornecedor",
                table: "produto",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fornecedor",
                table: "produto");
        }
    }
}
