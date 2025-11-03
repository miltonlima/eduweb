using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcSaed.Migrations
{
    /// <inheritdoc />
    public partial class TurmaUnidadeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnidadeId",
                table: "Turma",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turma_UnidadeId",
                table: "Turma",
                column: "UnidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turma_Unidade_UnidadeId",
                table: "Turma",
                column: "UnidadeId",
                principalTable: "Unidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turma_Unidade_UnidadeId",
                table: "Turma");

            migrationBuilder.DropIndex(
                name: "IX_Turma_UnidadeId",
                table: "Turma");

            migrationBuilder.DropColumn(
                name: "UnidadeId",
                table: "Turma");
        }
    }
}
