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
                .Include(t => t.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Modalidade)
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
        public async Task<IActionResult> Create([Bind("Id,Nome,DataInicio,DataFim,Status,UnidadeId")] Turma turma, int? SelectedModalidadeId)
        {
            if (ModelState.IsValid)
            {
                turma.DataCriacao = DateTime.Now;
                _context.Add(turma);
                await _context.SaveChangesAsync();

                // Vincula a modalidade selecionada (uma por turma)
                if (SelectedModalidadeId.HasValue && SelectedModalidadeId.Value > 0)
                {
                    var modalidadeExists = await _context.Modalidade.AnyAsync(m => m.Id == SelectedModalidadeId.Value);
                    if (!modalidadeExists)
                    {
                        ModelState.AddModelError("SelectedModalidadeId", "Modalidade inválida.");
                        return View(turma);
                    }

                    _context.ModalidadeTurma.Add(new ModalidadeTurma
                    {
                        ModalidadeId = SelectedModalidadeId.Value,
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataInicio,DataFim,Status,DataCriacao,UnidadeId")] Turma turma, int? SelectedModalidadeId)
        {
            if (id != turma.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(turma);

                    // Atualiza o vínculo da modalidade (garante no máximo uma modalidade por turma)
                    var currentLinks = await _context.ModalidadeTurma
                        .Where(mt => mt.TurmaId == turma.Id)
                        .ToListAsync();

                    if (SelectedModalidadeId.HasValue && SelectedModalidadeId.Value > 0)
                    {
                        var modalidadeExists = await _context.Modalidade.AnyAsync(m => m.Id == SelectedModalidadeId.Value);
                        if (!modalidadeExists)
                        {
                            ModelState.AddModelError("SelectedModalidadeId", "Modalidade inválida.");
                            return View(turma);
                        }

                        // Se já existir o mesmo vínculo, apenas remove os demais; senão recria
                        if (!currentLinks.Any(l => l.ModalidadeId == SelectedModalidadeId.Value))
                        {
                            _context.ModalidadeTurma.RemoveRange(currentLinks);
                            _context.ModalidadeTurma.Add(new ModalidadeTurma
                            {
                                ModalidadeId = SelectedModalidadeId.Value,
                                TurmaId = turma.Id
                            });
                        }
                        else
                        {
                            _context.ModalidadeTurma.RemoveRange(currentLinks.Where(l => l.ModalidadeId != SelectedModalidadeId.Value));
                        }
                    }
                    else
                    {
                        // Sem modalidade selecionada: remove vínculos existentes
                        if (currentLinks.Any())
                            _context.ModalidadeTurma.RemoveRange(currentLinks);
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
                .Include(t => t.ModalidadesTurmas)
                    .ThenInclude(mt => mt.Modalidade)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (turma == null) return NotFound();
            
            // Verifica se a turma possui modalidades vinculadas
            ViewBag.PossuiModalidades = turma.ModalidadesTurmas.Any();
            ViewBag.QuantidadeModalidades = turma.ModalidadesTurmas.Count;
            
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
                    .Include(t => t.ModalidadesTurmas)
                    .FirstOrDefaultAsync(t => t.Id == id);
                    
                if (turma == null) return NotFound();
                
                // Verifica se ainda possui modalidades vinculadas
                if (turma.ModalidadesTurmas.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir esta turma pois ela possui modalidades vinculadas. Primeiro desvincule todas as modalidades.";
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
