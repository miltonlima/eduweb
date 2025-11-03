using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MvcSaed.Data;
using MvcSaed.Models;

namespace MvcSaed.Controllers
{
    [Authorize]
    public class TurmasController : Controller
    {
        private readonly MvcSaedContext _context;

        public TurmasController(MvcSaedContext context)
        {
            _context = context;
        }

        // GET: Turmas
        public async Task<IActionResult> Index()
        {
            var turmas = await _context.Turma
                .Include(t => t.Unidade)
                .Include(t => t.CursosTurmas)
                    .ThenInclude(mt => mt.Curso)
                .ToListAsync();
            return View(turmas);
        }

        // GET: Turmas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var turma = await _context.Turma
                .Include(t => t.Unidade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turma == null)
                return NotFound();
            return View(turma);
        }

        // GET: Turmas/Create
        public async Task<IActionResult> Create()
        {
            var anoAtual = DateTime.Now.Year;
            var turma = new Turma
            {
                DataInicio = new DateTime(anoAtual, 1, 1),
                DataFim = new DateTime(anoAtual, 12, 31)
            };
            
            ViewBag.UnidadeId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                await _context.Unidade.Where(u => u.Ativa).OrderBy(u => u.Nome).ToListAsync(), 
                "Id", "Nome");
                
            return View(turma);
        }

        // POST: Turmas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataInicio,DataFim,Status,UnidadeId")] Turma turma, int? SelectedCursoId)
        {
            if (ModelState.IsValid)
            {
                turma.DataCriacao = DateTime.Now;
                _context.Add(turma);
                await _context.SaveChangesAsync();

                // Vincula o curso selecionado (um por turma)
                if (SelectedCursoId.HasValue && SelectedCursoId.Value > 0)
                {
                    var cursoExists = await _context.Curso.AnyAsync(m => m.Id == SelectedCursoId.Value);
                    if (!cursoExists)
                    {
                        ModelState.AddModelError("SelectedCursoId", "Curso inválido.");
                        return View(turma);
                    }

                    _context.CursoTurma.Add(new CursoTurma
                    {
                        CursoId = SelectedCursoId.Value,
                        TurmaId = turma.Id
                    });
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.UnidadeId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                await _context.Unidade.Where(u => u.Ativa).OrderBy(u => u.Nome).ToListAsync(), 
                "Id", "Nome", turma.UnidadeId);
                
            return View(turma);
        }

        // GET: Turmas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var turma = await _context.Turma.FindAsync(id);
            if (turma == null)
                return NotFound();
                
            ViewBag.UnidadeId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                await _context.Unidade.Where(u => u.Ativa).OrderBy(u => u.Nome).ToListAsync(), 
                "Id", "Nome", turma.UnidadeId);
                
            return View(turma);
        }

        // POST: Turmas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataInicio,DataFim,Status,DataCriacao,UnidadeId")] Turma turma, int? SelectedCursoId)
        {
            if (id != turma.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turma);

                    // Atualiza o vínculo do curso (garante no máximo um curso por turma)
                    var currentLinks = await _context.CursoTurma
                        .Where(mt => mt.TurmaId == turma.Id)
                        .ToListAsync();

                    if (SelectedCursoId.HasValue && SelectedCursoId.Value > 0)
                    {
                        var cursoExists = await _context.Curso.AnyAsync(m => m.Id == SelectedCursoId.Value);
                        if (!cursoExists)
                        {
                            ModelState.AddModelError("SelectedCursoId", "Curso inválido.");
                            return View(turma);
                        }

                        // Se já existir o mesmo vínculo, apenas remove os demais; senão recria
                        if (!currentLinks.Any(l => l.CursoId == SelectedCursoId.Value))
                        {
                            _context.CursoTurma.RemoveRange(currentLinks);
                            _context.CursoTurma.Add(new CursoTurma
                            {
                                CursoId = SelectedCursoId.Value,
                                TurmaId = turma.Id
                            });
                        }
                        else
                        {
                            _context.CursoTurma.RemoveRange(currentLinks.Where(l => l.CursoId != SelectedCursoId.Value));
                        }
                    }
                    else
                    {
                        // Sem curso selecionado: remove vínculos existentes
                        if (currentLinks.Any())
                            _context.CursoTurma.RemoveRange(currentLinks);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurmaExists(turma.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.UnidadeId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                await _context.Unidade.Where(u => u.Ativa).OrderBy(u => u.Nome).ToListAsync(), 
                "Id", "Nome", turma.UnidadeId);
                
            return View(turma);
        }

        // GET: Turmas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var turma = await _context.Turma
                .Include(t => t.CursosTurmas)
                    .ThenInclude(mt => mt.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (turma == null) return NotFound();
            
            // Verifica se a turma possui cursos vinculados
            ViewBag.PossuiCursos = turma.CursosTurmas.Any();
            ViewBag.QuantidadeCursos = turma.CursosTurmas.Count;
            
            return View(turma);
        }

        // POST: Turmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var turma = await _context.Turma
                    .Include(t => t.CursosTurmas)
                    .FirstOrDefaultAsync(t => t.Id == id);
                    
                if (turma == null) return NotFound();
                
                // Verifica se ainda possui cursos vinculados
                if (turma.CursosTurmas.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir esta turma pois ela possui cursos vinculados. Primeiro desvincule todos os cursos.";
                    return RedirectToAction(nameof(Index));
                }
                
                // Verifica se possui inscrições de estudantes
                var possuiInscricoes = await _context.InscricaoTurma.AnyAsync(i => i.TurmaId == id);
                if (possuiInscricoes)
                {
                    TempData["ErrorMessage"] = "Não é possível excluir esta turma pois ela possui estudantes inscritos. Primeiro remova todas as inscrições.";
                    return RedirectToAction(nameof(Index));
                }
                
                // Remove a turma (as associações são removidas automaticamente por cascade)
                _context.Turma.Remove(turma);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                TempData["SuccessMessage"] = "Turma excluída com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = $"Erro ao excluir turma: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TurmaExists(int id)
        {
            return _context.Turma.Any(e => e.Id == id);
        }
    }
}
