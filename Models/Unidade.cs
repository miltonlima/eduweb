using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcSaed.Models
{
    public class Unidade
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descricao { get; set; }

        [Display(Name = "Ativa")]
        public bool Ativa { get; set; } = true;

        [StringLength(500)]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "É obrigatório selecionar uma turma para a unidade.")]
        [Display(Name = "Turma")]
        public int TurmaId { get; set; }

        [ForeignKey("TurmaId")]
        public virtual Turma? Turma { get; set; }
    }
}
