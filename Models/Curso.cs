using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcSaed.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do curso é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome do Curso")]
        public string Nome { get; set; } = string.Empty;

        public ICollection<CursoTurma> CursosTurmas { get; set; } = new List<CursoTurma>();
    }
}
