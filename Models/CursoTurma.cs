namespace MvcSaed.Models
{
    public class CursoTurma
    {
        public int CursoId { get; set; }
        public Curso Curso { get; set; } = null!;
        
        public int TurmaId { get; set; }
        public Turma Turma { get; set; } = null!;
    }
}
