using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class AjustarRelacionamentoUsuarioSubcategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Categorias_CategoriaId",
                table: "Servicos");

            migrationBuilder.DropTable(
                name: "UsuarioCategorias");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Servicos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SubcategoriaId",
                table: "Servicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Subcategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuarioSubcategorias",
                columns: table => new
                {
                    EspecialidadesId = table.Column<int>(type: "int", nullable: false),
                    UsuariosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSubcategorias", x => new { x.EspecialidadesId, x.UsuariosId });
                    table.ForeignKey(
                        name: "FK_UsuarioSubcategorias_Subcategorias_EspecialidadesId",
                        column: x => x.EspecialidadesId,
                        principalTable: "Subcategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioSubcategorias_Usuarios_UsuariosId",
                        column: x => x.UsuariosId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_SubcategoriaId",
                table: "Servicos",
                column: "SubcategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategorias_CategoriaId",
                table: "Subcategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSubcategorias_UsuariosId",
                table: "UsuarioSubcategorias",
                column: "UsuariosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Categorias_CategoriaId",
                table: "Servicos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Subcategorias_SubcategoriaId",
                table: "Servicos",
                column: "SubcategoriaId",
                principalTable: "Subcategorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Categorias_CategoriaId",
                table: "Servicos");

            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Subcategorias_SubcategoriaId",
                table: "Servicos");

            migrationBuilder.DropTable(
                name: "UsuarioSubcategorias");

            migrationBuilder.DropTable(
                name: "Subcategorias");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_SubcategoriaId",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "SubcategoriaId",
                table: "Servicos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaId",
                table: "Servicos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UsuarioCategorias",
                columns: table => new
                {
                    EspecialidadesId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCategorias", x => new { x.EspecialidadesId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioCategorias_Categorias_EspecialidadesId",
                        column: x => x.EspecialidadesId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCategorias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCategorias_UsuarioId",
                table: "UsuarioCategorias",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Categorias_CategoriaId",
                table: "Servicos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
