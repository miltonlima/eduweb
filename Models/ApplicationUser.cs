using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MvcSaed.Models
{
    /// <summary>
    /// Modelo de usuário personalizado do SAED que estende IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Indica se o usuário está ativo no sistema
        /// </summary>
        [Display(Name = "Usuário Ativo")]
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data de criação do usuário
        /// </summary>
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        /// <summary>
        /// Data do último acesso do usuário
        /// </summary>
        [Display(Name = "Último Acesso")]
        public DateTime? UltimoAcesso { get; set; }

        /// <summary>
        /// Função/Cargo do usuário no sistema
        /// </summary>
        [StringLength(50, ErrorMessage = "A função deve ter no máximo 50 caracteres")]
        [Display(Name = "Função")]
        public string? Funcao { get; set; }

        /// <summary>
        /// Observações sobre o usuário
        /// </summary>
        [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres")]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        /// <summary>
        /// Avatar/Foto do usuário (URL ou caminho)
        /// </summary>
        [StringLength(255)]
        [Display(Name = "Avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// Número de tentativas de login falhadas
        /// </summary>
        public int TentativasLogin { get; set; } = 0;

        /// <summary>
        /// Data do último login bem-sucedido
        /// </summary>
        [Display(Name = "Último Login")]
        public DateTime? UltimoLogin { get; set; }
    }
}