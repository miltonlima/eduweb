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
    public class InscricoesController : Controller
    {
        private readonly MvcSaedContext _context;

        public InscricoesController(MvcSaedContext context)
        {
            _context = context;
        }

        // GET: Inscricoes
        public async Task<IActionResult> Index()
        {
            var inscricoes = _context.InscricaoTurma
                .Include(i => i.Pessoa)
                .Include(i => i.Turma);
            return View(await inscricoes.ToListAsync());
        }

        // GET: Inscricoes/Create
        public IActionResult Create()
        {
            ViewBag.Pessoas = _context.Pessoa.OrderBy(p => p.Nome).ToList();
            ViewBag.Turmas = _context.Turma.OrderBy(t => t.Nome).ToList();
            return View();
        }

        // POST: Inscricoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InscricaoTurma inscricao)
        {
            // Remove validação das propriedades de navegação
            ModelState.Remove("Pessoa");
            ModelState.Remove("Turma");
            
            // Validação manual dos IDs
            if (inscricao.PessoaId <= 0)
            {
                ModelState.AddModelError("PessoaId", "Selecione um aluno.");
            }
            
            if (inscricao.TurmaId <= 0)
            {
                ModelState.AddModelError("TurmaId", "Selecione uma turma.");
            }

            if (ModelState.IsValid)
            {
                // Prevent duplicate enrollment
                bool exists = await _context.InscricaoTurma.AnyAsync(i => i.PessoaId == inscricao.PessoaId && i.TurmaId == inscricao.TurmaId);
                if (exists)
                {
                    ModelState.AddModelError("", "Este aluno já está inscrito nesta turma.");
                    ViewBag.Pessoas = _context.Pessoa.OrderBy(p => p.Nome).ToList();
                    ViewBag.Turmas = _context.Turma.OrderBy(t => t.Nome).ToList();
                    return View(inscricao);
                }

                // Define a data de inscrição como agora
                inscricao.DataInscricao = DateTime.Now;
                
                _context.Add(inscricao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Pessoas = _context.Pessoa.OrderBy(p => p.Nome).ToList();
            ViewBag.Turmas = _context.Turma.OrderBy(t => t.Nome).ToList();
            return View(inscricao);
        }

        // POST: Inscricoes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var inscricao = await _context.InscricaoTurma.FindAsync(id);
            if (inscricao != null)
            {
                _context.InscricaoTurma.Remove(inscricao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
