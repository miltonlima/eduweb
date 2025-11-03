using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MvcSaed.Data;
using MvcSaed.Models;

namespace MvcSaed.Controllers
{
    [Authorize]
    public class UnidadesController : Controller
    {
        private readonly MvcSaedContext _context;

        public UnidadesController(MvcSaedContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var unidades = await _context.Unidade
                .Include(u => u.Turma)
                .OrderBy(u => u.Nome)
                .ToListAsync();
            return View(unidades);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.TurmaId = new SelectList(await _context.Turma.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Ativa,Endereco,TurmaId")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TurmaId = new SelectList(await _context.Turma.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", unidade.TurmaId);
            return View(unidade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var unidade = await _context.Unidade.FindAsync(id);
            if (unidade == null) return NotFound();
            ViewBag.TurmaId = new SelectList(await _context.Turma.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", unidade.TurmaId);
            return View(unidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Ativa,Endereco,TurmaId")] Unidade unidade)
        {
            if (id != unidade.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Unidade.AnyAsync(e => e.Id == unidade.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TurmaId = new SelectList(await _context.Turma.OrderBy(t => t.Nome).ToListAsync(), "Id", "Nome", unidade.TurmaId);
            return View(unidade);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var unidade = await _context.Unidade
                .Include(u => u.Turma)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (unidade == null) return NotFound();
            return View(unidade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var unidade = await _context.Unidade
                .Include(u => u.Turma)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (unidade == null) return NotFound();

            return View(unidade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var unidade = await _context.Unidade.FindAsync(id);
            if (unidade != null)
            {
                _context.Unidade.Remove(unidade);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
