namespace MvcSaed.Models
{
    public class ModalidadeTurma
    {
        public int ModalidadeId { get; set; }
        public Modalidade Modalidade { get; set; } = null!;
        
        public int TurmaId { get; set; }
        public Turma Turma { get; set; } = null!;
    }
}
