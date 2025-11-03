
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcSaed.Models;
using MvcSaed.Data;

namespace MvcSaed.Services
{
    public class ModalidadeService
    {
        private readonly MvcSaedContext _context;

        public ModalidadeService(MvcSaedContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteCicloEntreModalidades(int modalidadeId, int[] turmaIds)
        {
            // Se não há turmas selecionadas, não há ciclo
            if (turmaIds == null || turmaIds.Length == 0)
                return false;

            // Busca todas as turmas selecionadas que já estão associadas a outras modalidades
            var turmasComOutrasModalidades = await _context.ModalidadeTurma
                .Where(mt => turmaIds.Contains(mt.TurmaId) && mt.ModalidadeId != modalidadeId)
                .AnyAsync();

            return turmasComOutrasModalidades;
        }
    }
}
