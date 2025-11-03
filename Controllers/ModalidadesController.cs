using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcSaed.Data;
using MvcSaed.Models;
using MvcSaed.Services;

namespace MvcSaed.Controllers
{
    [Authorize]
    public class ModalidadesController : Controller
    {
        private readonly MvcSaedContext _context;
        private readonly ModalidadeService _modalidadeService;

        public ModalidadesController(MvcSaedContext context)
        {
            _context = context;
            _modalidadeService = new ModalidadeService(context);
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Modalidade
                .Include(m => m.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Turma)
                .ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var modalidade = await _context.Modalidade
                .Include(m => m.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidade == null) return NotFound();
            return View(modalidade);
        }

        public IActionResult Create()
        {
            // Seleciona apenas turmas que não estão em nenhuma modalidade
            var turmasNaoAtribuidas = _context.Turma
                .Where(t => !_context.ModalidadeTurma.Any(mt => mt.TurmaId == t.Id))
                .ToList();
            ViewBag.Turmas = turmasNaoAtribuidas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Modalidade modalidade, int[] selectedTurmas)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (selectedTurmas != null && selectedTurmas.Length > 0)
                    {
                        // Verifica se alguma das turmas já tem modalidade
                        if (await _modalidadeService.ExisteCicloEntreModalidades(modalidade.Id, selectedTurmas))
                        {
                            ModelState.AddModelError("", "Uma ou mais turmas selecionadas já estão vinculadas a outras modalidades. Não é possível vincular a mesma turma a mais de uma modalidade.");
                        }
                        else
                        {
                            // Adiciona a modalidade primeiro
                            _context.Add(modalidade);
                            await _context.SaveChangesAsync();

                            // Agora adiciona as associações
                            foreach (var turmaId in selectedTurmas)
                            {
                                // Verifica novamente se a turma ainda está disponível (concorrência)
                                var turmaJaVinculada = await _context.ModalidadeTurma
                                    .AnyAsync(mt => mt.TurmaId == turmaId);
                                
                                if (!turmaJaVinculada)
                                {
                                    _context.ModalidadeTurma.Add(new ModalidadeTurma
                                    {
                                        ModalidadeId = modalidade.Id,
                                        TurmaId = turmaId
                                    });
                                }
                            }
                            
                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        _context.Add(modalidade);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }

            // Recarrega as turmas disponíveis
            ViewBag.Turmas = await _context.Turma
                .Where(t => !_context.ModalidadeTurma.Any(mt => mt.TurmaId == t.Id))
                .ToListAsync();
            return View(modalidade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var modalidade = await _context.Modalidade
                .Include(m => m.ModalidadesTurmas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidade == null) return NotFound();

            // Turmas já atribuídas a esta modalidade
            var turmasAtribuidas = modalidade.ModalidadesTurmas.Select(mt => mt.TurmaId).ToList();
            // Turmas não atribuídas a nenhuma modalidade ou já atribuídas a esta modalidade
            var turmasDisponiveis = _context.Turma
                .Where(t => !_context.ModalidadeTurma.Any(mt => mt.TurmaId == t.Id && mt.ModalidadeId != modalidade.Id))
                .ToList();
            ViewBag.Turmas = turmasDisponiveis;
            ViewBag.SelectedTurmas = turmasAtribuidas;
            return View(modalidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Modalidade modalidade, int[] selectedTurmas)
        {
            if (id != modalidade.Id) return NotFound();
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var modalidadeToUpdate = await _context.Modalidade
                        .Include(m => m.ModalidadesTurmas)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (modalidadeToUpdate == null) return NotFound();

                    // Verifica se alguma das novas turmas selecionadas já tem modalidade
                    // excluindo as turmas que já pertencem a esta modalidade
                    var turmasAtuais = modalidadeToUpdate.ModalidadesTurmas.Select(mt => mt.TurmaId).ToArray();
                    var novasTurmas = selectedTurmas?.Except(turmasAtuais).ToArray() ?? Array.Empty<int>();

                    if (novasTurmas.Length > 0 && await _modalidadeService.ExisteCicloEntreModalidades(modalidade.Id, novasTurmas))
                    {
                        ModelState.AddModelError("", "Uma ou mais turmas selecionadas já estão vinculadas a outras modalidades. Não é possível vincular a mesma turma a mais de uma modalidade.");
                        await ReloadEditViewData(modalidade.Id, selectedTurmas ?? Array.Empty<int>());
                        return View(modalidade);
                    }

                    // Remove associações antigas
                    _context.ModalidadeTurma.RemoveRange(modalidadeToUpdate.ModalidadesTurmas);
                    
                    // Atualiza o nome da modalidade
                    modalidadeToUpdate.Nome = modalidade.Nome;

                    // Adiciona novas associações
                    if (selectedTurmas != null && selectedTurmas.Length > 0)
                    {
                        foreach (var turmaId in selectedTurmas)
                        {
                            // Verifica se a turma ainda está disponível (proteção contra concorrência)
                            var turmaDisponivel = await _context.Turma
                                .Where(t => t.Id == turmaId)
                                .Where(t => !_context.ModalidadeTurma.Any(mt => mt.TurmaId == t.Id && mt.ModalidadeId != modalidade.Id))
                                .AnyAsync();

                            if (turmaDisponivel)
                            {
                                _context.ModalidadeTurma.Add(new ModalidadeTurma
                                {
                                    ModalidadeId = modalidade.Id,
                                    TurmaId = turmaId
                                });
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    if (!await _context.Set<Modalidade>().AnyAsync(e => e.Id == modalidade.Id)) 
                        return NotFound();
                    ModelState.AddModelError("", "Erro de concorrência: a modalidade foi modificada por outro usuário.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            
            await ReloadEditViewData(modalidade.Id, selectedTurmas ?? Array.Empty<int>());
            return View(modalidade);
        }

        private async Task ReloadEditViewData(int modalidadeId, int[] selectedTurmas)
        {
            // Turmas não atribuídas a nenhuma modalidade ou já atribuídas a esta modalidade
            var turmasDisponiveis = await _context.Turma
                .Where(t => !_context.ModalidadeTurma.Any(mt => mt.TurmaId == t.Id && mt.ModalidadeId != modalidadeId))
                .ToListAsync();
            ViewBag.Turmas = turmasDisponiveis;
            ViewBag.SelectedTurmas = selectedTurmas ?? Array.Empty<int>();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var modalidade = await _context.Modalidade
                .Include(m => m.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (modalidade == null) return NotFound();
            
            // Verifica se a modalidade possui turmas vinculadas
            ViewBag.PossuiTurmas = modalidade.ModalidadesTurmas.Any();
            ViewBag.QuantidadeTurmas = modalidade.ModalidadesTurmas.Count;
            
            return View(modalidade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var modalidade = await _context.Modalidade
                    .Include(m => m.ModalidadesTurmas)
                    .FirstOrDefaultAsync(m => m.Id == id);
                    
                if (modalidade == null) return NotFound();
                
                // Verifica se ainda possui turmas vinculadas
                if (modalidade.ModalidadesTurmas.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir esta modalidade pois ela possui turmas vinculadas. Primeiro desvincule todas as turmas.";
                    return RedirectToAction(nameof(Index));
                }
                
                // Remove a modalidade (as associações são removidas automaticamente por cascade)
                _context.Modalidade.Remove(modalidade);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                TempData["SuccessMessage"] = "Modalidade excluída com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Erro ao excluir modalidade: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
