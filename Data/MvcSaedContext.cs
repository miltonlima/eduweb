using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcSaed.Models;

namespace MvcSaed.Data
{
    public class MvcSaedContext : IdentityDbContext<ApplicationUser>
    {
        public MvcSaedContext (DbContextOptions<MvcSaedContext> options)
            : base(options)
        {
        }

    public DbSet<MvcSaed.Models.Movie> Movie { get; set; } = default!;
    public DbSet<Pessoa> Pessoa { get; set; }
    public DbSet<Turma> Turma { get; set; }
    public DbSet<MvcSaed.Models.Curso> Curso { get; set; } = default!;
    public DbSet<MvcSaed.Models.CursoTurma> CursoTurma { get; set; } = default!;
    public DbSet<InscricaoTurma> InscricaoTurma { get; set; }
    // Unidades
    public DbSet<MvcSaed.Models.Unidade> Unidade { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Garantir que o enum StatusTurma seja persistido como INT
        modelBuilder.Entity<MvcSaed.Models.Turma>()
            .Property(t => t.Status)
            .HasConversion<int>();

        // DataCriacao gerada pelo banco (assume coluna DATETIME / TIMESTAMP com DEFAULT CURRENT_TIMESTAMP)
        modelBuilder.Entity<MvcSaed.Models.Turma>()
            .Property(t => t.DataCriacao)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Configuração do relacionamento many-to-many entre Curso e Turma
        modelBuilder.Entity<MvcSaed.Models.CursoTurma>(entity =>
        {
            entity.ToTable("curso_turma");
            entity.HasKey(mt => new { mt.CursoId, mt.TurmaId });

            entity.HasOne(mt => mt.Curso)
                .WithMany(m => m.CursosTurmas)
                .HasForeignKey(mt => mt.CursoId);

            entity.HasOne(mt => mt.Turma)
                .WithMany(t => t.CursosTurmas)
                .HasForeignKey(mt => mt.TurmaId);
        });

        modelBuilder.Entity<Pessoa>().ToTable("Pessoa");
        modelBuilder.Entity<Turma>().ToTable("Turma");
        modelBuilder.Entity<InscricaoTurma>().ToTable("InscricaoTurma");

        modelBuilder.Entity<InscricaoTurma>()
            .HasIndex(i => new { i.PessoaId, i.TurmaId })
            .IsUnique();

        modelBuilder.Entity<InscricaoTurma>()
            .HasOne(i => i.Pessoa)
            .WithMany(p => p.Inscricoes)
            .HasForeignKey(i => i.PessoaId);

        modelBuilder.Entity<InscricaoTurma>()
            .HasOne(i => i.Turma)
            .WithMany(t => t.Inscricoes)
            .HasForeignKey(i => i.TurmaId);

        // Unidades table mapping with relationship to Turma
        modelBuilder.Entity<MvcSaed.Models.Unidade>(entity =>
        {
            entity.ToTable("Unidade");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Nome).HasMaxLength(150).IsRequired();
            entity.Property(u => u.Descricao).HasMaxLength(1000);
            entity.Property(u => u.Endereco).HasMaxLength(500);
            entity.Property(u => u.Ativa).HasDefaultValue(true);
            entity.Property(u => u.TurmaId).IsRequired();

            // Configurar relacionamento com Turma
            entity.HasOne(u => u.Turma)
                .WithMany(t => t.Unidades)
                .HasForeignKey(u => u.TurmaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurar relacionamento adicional: Turma pode ter uma Unidade principal
        modelBuilder.Entity<MvcSaed.Models.Turma>(entity =>
        {
            entity.HasOne(t => t.Unidade)
                .WithMany()
                .HasForeignKey(t => t.UnidadeId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
    }
}
