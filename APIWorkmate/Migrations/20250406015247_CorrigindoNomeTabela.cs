using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoNomeTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Servicios_ServicoId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Contratacoes_Servicios_ServicoId",
                table: "Contratacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicios_Categorias_CategoriaId",
                table: "Servicios");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicios_Usuarios_PrestadorId",
                table: "Servicios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicios",
                table: "Servicios");

            migrationBuilder.RenameTable(
                name: "Servicios",
                newName: "Servicos");

            migrationBuilder.RenameIndex(
                name: "IX_Servicios_PrestadorId",
                table: "Servicos",
                newName: "IX_Servicos_PrestadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Servicios_CategoriaId",
                table: "Servicos",
                newName: "IX_Servicos_CategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicos",
                table: "Servicos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Servicos_ServicoId",
                table: "Avaliacoes",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contratacoes_Servicos_ServicoId",
                table: "Contratacoes",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Categorias_CategoriaId",
                table: "Servicos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Usuarios_PrestadorId",
                table: "Servicos",
                column: "PrestadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Servicos_ServicoId",
                table: "Avaliacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Contratacoes_Servicos_ServicoId",
                table: "Contratacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Categorias_CategoriaId",
                table: "Servicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Usuarios_PrestadorId",
                table: "Servicos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicos",
                table: "Servicos");

            migrationBuilder.RenameTable(
                name: "Servicos",
                newName: "Servicios");

            migrationBuilder.RenameIndex(
                name: "IX_Servicos_PrestadorId",
                table: "Servicios",
                newName: "IX_Servicios_PrestadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Servicos_CategoriaId",
                table: "Servicios",
                newName: "IX_Servicios_CategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicios",
                table: "Servicios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Servicios_ServicoId",
                table: "Avaliacoes",
                column: "ServicoId",
                principalTable: "Servicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contratacoes_Servicios_ServicoId",
                table: "Contratacoes",
                column: "ServicoId",
                principalTable: "Servicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Servicios_Categorias_CategoriaId",
                table: "Servicios",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Servicios_Usuarios_PrestadorId",
                table: "Servicios",
                column: "PrestadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
