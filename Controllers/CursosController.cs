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
    public class CursosController : Controller
    {
        private readonly MvcSaedContext _context;
        private readonly CursoService _cursoService;

        public CursosController(MvcSaedContext context)
        {
            _context = context;
            _cursoService = new CursoService(context);
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Curso
                .Include(m => m.CursosTurmas)
                    .ThenInclude(mt => mt.Turma)
                .ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var curso = await _context.Curso
                .Include(m => m.CursosTurmas)
                    .ThenInclude(mt => mt.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null) return NotFound();
            return View(curso);
        }

        public IActionResult Create()
        {
            // Seleciona apenas turmas que não estão em nenhum curso
            var turmasNaoAtribuidas = _context.Turma
                .Where(t => !_context.CursoTurma.Any(mt => mt.TurmaId == t.Id))
                .ToList();
            ViewBag.Turmas = turmasNaoAtribuidas;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Curso curso, int[] selectedTurmas)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (selectedTurmas != null && selectedTurmas.Length > 0)
                    {
                        // Verifica se alguma das turmas já tem curso
                        if (await _cursoService.ExisteCicloEntreCursos(curso.Id, selectedTurmas))
                        {
                            ModelState.AddModelError("", "Uma ou mais turmas selecionadas já estão vinculadas a outros cursos. Não é possível vincular a mesma turma a mais de um curso.");
                        }
                        else
                        {
                            // Adiciona o curso primeiro
                            _context.Add(curso);
                            await _context.SaveChangesAsync();

                            // Agora adiciona as associações
                            foreach (var turmaId in selectedTurmas)
                            {
                                // Verifica novamente se a turma ainda está disponível (concorrência)
                                var turmaJaVinculada = await _context.CursoTurma
                                    .AnyAsync(mt => mt.TurmaId == turmaId);
                                
                                if (!turmaJaVinculada)
                                {
                                    _context.CursoTurma.Add(new CursoTurma
                                    {
                                        CursoId = curso.Id,
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
                        _context.Add(curso);
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
                .Where(t => !_context.CursoTurma.Any(mt => mt.TurmaId == t.Id))
                .ToListAsync();
            return View(curso);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var curso = await _context.Curso
                .Include(m => m.CursosTurmas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curso == null) return NotFound();

            // Turmas já atribuídas a este curso
            var turmasAtribuidas = curso.CursosTurmas.Select(mt => mt.TurmaId).ToList();
            // Turmas não atribuídas a nenhum curso ou já atribuídas a este curso
            var turmasDisponiveis = _context.Turma
                .Where(t => !_context.CursoTurma.Any(mt => mt.TurmaId == t.Id && mt.CursoId != curso.Id))
                .ToList();
            ViewBag.Turmas = turmasDisponiveis;
            ViewBag.SelectedTurmas = turmasAtribuidas;
            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Curso curso, int[] selectedTurmas)
        {
            if (id != curso.Id) return NotFound();
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var cursoToUpdate = await _context.Curso
                        .Include(m => m.CursosTurmas)
                        .FirstOrDefaultAsync(m => m.Id == id);

                    if (cursoToUpdate == null) return NotFound();

                    // Verifica se alguma das novas turmas selecionadas já tem curso
                    // excluindo as turmas que já pertencem a este curso
                    var turmasAtuais = cursoToUpdate.CursosTurmas.Select(mt => mt.TurmaId).ToArray();
                    var novasTurmas = selectedTurmas?.Except(turmasAtuais).ToArray() ?? Array.Empty<int>();

                    if (novasTurmas.Length > 0 && await _cursoService.ExisteCicloEntreCursos(curso.Id, novasTurmas))
                    {
                        ModelState.AddModelError("", "Uma ou mais turmas selecionadas já estão vinculadas a outros cursos. Não é possível vincular a mesma turma a mais de um curso.");
                        await ReloadEditViewData(curso.Id, selectedTurmas ?? Array.Empty<int>());
                        return View(curso);
                    }

                    // Remove associações antigas
                    _context.CursoTurma.RemoveRange(cursoToUpdate.CursosTurmas);
                    
                    // Atualiza o nome do curso
                    cursoToUpdate.Nome = curso.Nome;

                    // Adiciona novas associações
                    if (selectedTurmas != null && selectedTurmas.Length > 0)
                    {
                        foreach (var turmaId in selectedTurmas)
                        {
                            // Verifica se a turma ainda está disponível (proteção contra concorrência)
                            var turmaDisponivel = await _context.Turma
                                .Where(t => t.Id == turmaId)
                                .Where(t => !_context.CursoTurma.Any(mt => mt.TurmaId == t.Id && mt.CursoId != curso.Id))
                                .AnyAsync();

                            if (turmaDisponivel)
                            {
                                _context.CursoTurma.Add(new CursoTurma
                                {
                                    CursoId = curso.Id,
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
                    if (!await _context.Set<Curso>().AnyAsync(e => e.Id == curso.Id)) 
                        return NotFound();
                    ModelState.AddModelError("", "Erro de concorrência: o curso foi modificado por outro usuário.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            
            await ReloadEditViewData(curso.Id, selectedTurmas ?? Array.Empty<int>());
            return View(curso);
        }

        private async Task ReloadEditViewData(int cursoId, int[] selectedTurmas)
        {
            // Turmas não atribuídas a nenhum curso ou já atribuídas a este curso
            var turmasDisponiveis = await _context.Turma
                .Where(t => !_context.CursoTurma.Any(mt => mt.TurmaId == t.Id && mt.CursoId != cursoId))
                .ToListAsync();
            ViewBag.Turmas = turmasDisponiveis;
            ViewBag.SelectedTurmas = selectedTurmas ?? Array.Empty<int>();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var curso = await _context.Curso
                .Include(m => m.CursosTurmas)
                    .ThenInclude(mt => mt.Turma)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (curso == null) return NotFound();
            
            // Verifica se o curso possui turmas vinculadas
            ViewBag.PossuiTurmas = curso.CursosTurmas.Any();
            ViewBag.QuantidadeTurmas = curso.CursosTurmas.Count;
            
            return View(curso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var curso = await _context.Curso
                    .Include(m => m.CursosTurmas)
                    .FirstOrDefaultAsync(m => m.Id == id);
                    
                if (curso == null) return NotFound();
                
                // Verifica se ainda possui turmas vinculadas
                if (curso.CursosTurmas.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir este curso pois ele possui turmas vinculadas. Primeiro desvincule todas as turmas.";
                    return RedirectToAction(nameof(Index));
                }
                
                // Remove o curso (as associações são removidas automaticamente por cascade)
                _context.Curso.Remove(curso);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                TempData["SuccessMessage"] = "Curso excluído com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Erro ao excluir curso: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
