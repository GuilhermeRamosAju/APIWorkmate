using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIWorkmate.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTipoUsuarioToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "Tipo",
              table: "Usuarios",
              type: "varchar(50)",
              nullable: false,
              oldClrType: typeof(int),
              oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
