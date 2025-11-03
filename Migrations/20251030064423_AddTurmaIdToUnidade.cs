using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcSaed.Migrations
{
    /// <inheritdoc />
    public partial class AddTurmaIdToUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurmaId",
                table: "Unidade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Unidade_TurmaId",
                table: "Unidade",
                column: "TurmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Unidade_Turma_TurmaId",
                table: "Unidade",
                column: "TurmaId",
                principalTable: "Turma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unidade_Turma_TurmaId",
                table: "Unidade");

            migrationBuilder.DropIndex(
                name: "IX_Unidade_TurmaId",
                table: "Unidade");

            migrationBuilder.DropColumn(
                name: "TurmaId",
                table: "Unidade");
        }
    }
}
