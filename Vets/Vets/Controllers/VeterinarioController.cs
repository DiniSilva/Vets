using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vets.Data;
using Vets.Models;

namespace Vets.Controllers
{
    public class VeterinarioController : Controller
    {
        /// <summary>
        /// cria uma instancia de acesso á Base de Dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        public VeterinarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veterinario
        public async Task<IActionResult> Index()
        {
            /* acesso á base de dados
             * SELECT *
             * FROM veterinario
             * 
             * 
             */
            return View(await _context.Veterinarios.ToListAsync());
        }

        // GET: Veterinario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // GET: Veterinario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veterinario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios, IFormFile fotoVet){
            /*
             * Algorismo para processar as imagens
             * 
             * se ficheiro da imagem for nulo
             *      atribuir uma imagem genérica ao veterinário
             * else
             *      será que o ficheiro é uma imagem?
             *      se não for
             *          criar mensagem de erro
             *          devolver o controlo da app á view
             *      else
             *          - definir o nome a atribuir á imagem
             *          - atribuir aos dados do novo vet, o nome do ficheiro da imagem
             *          - guardar a imagem no disco rígido do servidor
             */
            
            if(fotoVet == null) {
                veterinarios.Fotografia = "noVet.png";
            }
            else {
                if(!(fotoVet.ContentType == "image/png" || fotoVet.ContentType == "image/jpeg")) {
                    // criar mensagem de erro
                    ModelState.AddModelError("", "Por favor, adicione um ficheiro .png ou .jpg");
                    //resolver o controlo da app á View
                    return View(veterinarios);
                } else {
                    //temos ficheiro e é uma imagem...





                }
            }

            
            if (ModelState.IsValid)
            {
                _context.Add(veterinarios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veterinarios);
        }

        // GET: Veterinario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios.FindAsync(id);
            if (veterinarios == null)
            {
                return NotFound();
            }
            return View(veterinarios);
        }

        // POST: Veterinario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinarios)
        {
            if (id != veterinarios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veterinarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeterinariosExists(veterinarios.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(veterinarios);
        }

        // GET: Veterinario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veterinarios = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veterinarios == null)
            {
                return NotFound();
            }

            return View(veterinarios);
        }

        // POST: Veterinario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veterinarios = await _context.Veterinarios.FindAsync(id);
            _context.Veterinarios.Remove(veterinarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeterinariosExists(int id)
        {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
