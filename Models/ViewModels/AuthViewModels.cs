using System.ComponentModel.DataAnnotations;

namespace MvcSaed.Models.ViewModels
{
    /// <summary>
    /// ViewModel para a página de login
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Digite um email válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Lembrar de mim")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }

    /// <summary>
    /// ViewModel para a página de registro
    /// </summary>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Digite um email válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação não conferem.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "A função deve ter no máximo 50 caracteres")]
        [Display(Name = "Função")]
        public string? Funcao { get; set; }

        [Display(Name = "Telefone")]
        [Phone(ErrorMessage = "Digite um número de telefone válido")]
        public string? PhoneNumber { get; set; }
    }

    /// <summary>
    /// ViewModel para gerenciar perfil do usuário
    /// </summary>
    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Digite um email válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefone")]
        [Phone(ErrorMessage = "Digite um número de telefone válido")]
        public string? PhoneNumber { get; set; }

        [StringLength(50, ErrorMessage = "A função deve ter no máximo 50 caracteres")]
        [Display(Name = "Função")]
        public string? Funcao { get; set; }

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres")]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Último Acesso")]
        public DateTime? UltimoAcesso { get; set; }

        [Display(Name = "Último Login")]
        public DateTime? UltimoLogin { get; set; }
    }

    /// <summary>
    /// ViewModel para alteração de senha
    /// </summary>
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "A senha atual é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não conferem.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}