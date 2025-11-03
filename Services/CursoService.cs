
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcSaed.Models;
using MvcSaed.Data;

namespace MvcSaed.Services
{
    public class CursoService
    {
        private readonly MvcSaedContext _context;

        public CursoService(MvcSaedContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteCicloEntreCursos(int cursoId, int[] turmaIds)
        {
            // Se não há turmas selecionadas, não há ciclo
            if (turmaIds == null || turmaIds.Length == 0)
                return false;

            // Busca todas as turmas selecionadas que já estão associadas a outros cursos
            var turmasComOutrosCursos = await _context.CursoTurma
                .Where(mt => turmaIds.Contains(mt.TurmaId) && mt.CursoId != cursoId)
                .AnyAsync();

            return turmasComOutrosCursos;
        }
    }
}
