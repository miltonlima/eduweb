using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcSaed.Data;
using MvcSaed.Models;

namespace MvcSaed.Controllers
{
    [Authorize]
    public class PessoasController : Controller
    {
        private readonly MvcSaedContext _context;

        public PessoasController(MvcSaedContext context)
        {
            _context = context;
        }

        // GET: Pessoas
        public async Task<IActionResult> Index()
        {
            return _context.Pessoa != null ? 
                        View(await _context.Pessoa.ToListAsync()) :
                        Problem("Entity set 'MvcSaedContext.Pessoa'  is null.");
        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoas/Create
        public async Task<IActionResult> Create()
        {
            var model = new Pessoa
            {
                Matricula = await GenerateNextMatriculaAsync()
            };
            return View(model);
        }

        // POST: Pessoas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Nascimento,Cpf,Email")] Pessoa pessoa)
        {
            pessoa.Matricula = await GenerateNextMatriculaAsync();

            ModelState.Remove(nameof(Pessoa.Matricula)); // evita erro de Required

            if (ModelState.IsValid)
            {
                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Nascimento,Cpf,Email")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Pessoa.Matricula)); // evita erro de Required

            if (!ModelState.IsValid)
            {
                var current = await _context.Pessoa.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (current != null) pessoa.Matricula = current.Matricula;
                return View(pessoa);
            }

            var existing = await _context.Pessoa.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Nome = pessoa.Nome;
            existing.Nascimento = pessoa.Nascimento;
            existing.Cpf = pessoa.Cpf;
            existing.Email = pessoa.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoaExists(existing.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pessoa == null)
            {
                return Problem("Entity set 'MvcSaedContext.Pessoa'  is null.");
            }
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa != null)
            {
                _context.Pessoa.Remove(pessoa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PessoaExists(int id)
        {
          return (_context.Pessoa?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Gera matrícula no formato yyyymmdd + sequência de 5 dígitos
        private async Task<string> GenerateNextMatriculaAsync()
        {
            var prefix = DateTime.Now.ToString("yyyyMMdd");
            var last = await _context.Pessoa
                .Where(p => p.Matricula.StartsWith(prefix))
                .Select(p => p.Matricula)
                .OrderByDescending(m => m)
                .FirstOrDefaultAsync();

            var lastSeq = 0;
            if (!string.IsNullOrEmpty(last) && last.Length > prefix.Length)
            {
                _ = int.TryParse(last.Substring(prefix.Length), out lastSeq);
            }

            var next = prefix + (lastSeq + 1).ToString("D5");
            return next;
        }
    }
}
