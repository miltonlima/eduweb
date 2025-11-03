using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcSaed.Models
{
    public class Modalidade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da modalidade é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Nome da Modalidade")]
        public string Nome { get; set; } = string.Empty;

        public ICollection<ModalidadeTurma> ModalidadesTurmas { get; set; } = new List<ModalidadeTurma>();
    }
}
