using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcSaed.Attributes;

namespace MvcSaed.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime Nascimento { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres")]
        [CpfValidation]
        [Display(Name = "CPF")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A matrícula é obrigatória")]
        [StringLength(15, ErrorMessage = "A matrícula deve ter no máximo 15 números")]
        [Display(Name = "Matrícula")]
        public string Matricula { get; set; } = string.Empty;

        public ICollection<InscricaoTurma> Inscricoes { get; set; } = new List<InscricaoTurma>();
    }
}
