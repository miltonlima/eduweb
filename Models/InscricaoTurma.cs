using System;
using System.ComponentModel.DataAnnotations;

namespace MvcSaed.Models
{
    public class InscricaoTurma
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione um aluno.")]
        [Display(Name = "Aluno")]
        public int PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }

        [Required(ErrorMessage = "Selecione uma turma.")]
        [Display(Name = "Turma")]
        public int TurmaId { get; set; }
        public Turma? Turma { get; set; }

        [Display(Name = "Data de Inscrição")]
        public DateTime DataInscricao { get; set; }
    }
}
