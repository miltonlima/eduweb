using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["TurmaSortParm"] = sortOrder == "turma_asc" ? "turma_desc" : "turma_asc";
            ViewData["AtivaSortParm"] = sortOrder == "ativa_asc" ? "ativa_desc" : "ativa_asc";
            ViewData["EnderecoSortParm"] = sortOrder == "endereco_asc" ? "endereco_desc" : "endereco_asc";

            var unidades = from u in _context.Unidade.Include(u => u.Turma) select u;

            switch (sortOrder)
            {
                case "id_desc":
                    unidades = unidades.OrderByDescending(u => u.Id);
                    break;
                case "name_asc":
                    unidades = unidades.OrderBy(u => u.Nome);
                    break;
                case "name_desc":
                    unidades = unidades.OrderByDescending(u => u.Nome);
                    break;
                case "turma_asc":
                    unidades = unidades.OrderBy(u => u.Turma.Nome);
                    break;
                case "turma_desc":
                    unidades = unidades.OrderByDescending(u => u.Turma.Nome);
                    break;
                case "ativa_asc":
                    unidades = unidades.OrderBy(u => u.Ativa);
                    break;
                case "ativa_desc":
                    unidades = unidades.OrderByDescending(u => u.Ativa);
                    break;
                case "endereco_asc":
                    unidades = unidades.OrderBy(u => u.Endereco);
                    break;
                case "endereco_desc":
                    unidades = unidades.OrderByDescending(u => u.Endereco);
                    break;
                default: // id_asc
                    unidades = unidades.OrderBy(u => u.Id);
                    break;
            }

            return View(await unidades.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var unidade = await _context.Unidade.Include(u => u.Turma).FirstOrDefaultAsync(m => m.Id == id);
            if (unidade == null) return NotFound();
            return View(unidade);
        }

        public IActionResult Create()
        {
            ViewBag.TurmaId = new SelectList(_context.Turma, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Endereco,TurmaId,Ativa")] Unidade unidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TurmaId = new SelectList(_context.Turma, "Id", "Nome", unidade.TurmaId);
            return View(unidade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var unidade = await _context.Unidade.FindAsync(id);
            if (unidade == null) return NotFound();
            ViewBag.TurmaId = new SelectList(_context.Turma, "Id", "Nome", unidade.TurmaId);
            return View(unidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Endereco,TurmaId,Ativa")] Unidade unidade)
        {
            if (id != unidade.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(unidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TurmaId = new SelectList(_context.Turma, "Id", "Nome", unidade.TurmaId);
            return View(unidade);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var unidade = await _context.Unidade.Include(u => u.Turma).FirstOrDefaultAsync(m => m.Id == id);
            if (unidade == null) return NotFound();
            return View(unidade);
        }
    }
}