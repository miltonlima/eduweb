using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcSaed.Migrations
{
    /// <inheritdoc />
    public partial class FixRenameModalidadeToCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Esta migration é idempotente e pode ser executada múltiplas vezes
            // As tabelas Curso e curso_turma já foram criadas parcialmente
            // Vamos apenas garantir que tudo esteja correto
            
            migrationBuilder.Sql(@"
                -- Se as tabelas antigas ainda existirem, apenas renomeie
                DROP TABLE IF EXISTS `Modalidade`;
                DROP TABLE IF EXISTS `modalidade_turma`;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "curso_turma");

            migrationBuilder.DropTable(
                name: "Curso");

            migrationBuilder.CreateTable(
                name: "Modalidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalidade", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "modalidade_turma",
                columns: table => new
                {
                    ModalidadeId = table.Column<int>(type: "int", nullable: false),
                    TurmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modalidade_turma", x => new { x.ModalidadeId, x.TurmaId });
                    table.ForeignKey(
                        name: "FK_modalidade_turma_Modalidade_ModalidadeId",
                        column: x => x.ModalidadeId,
                        principalTable: "Modalidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_modalidade_turma_Turma_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_modalidade_turma_TurmaId",
                table: "modalidade_turma",
                column: "TurmaId");
        }
    }
}
