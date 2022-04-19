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

        /// <summary>
        /// esta variável vai conter os dados do servidor 
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VeterinarioController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                    //+++++++++++++++++++++++++++++++
                    //definir o nome da foto
                    Guid g = Guid.NewGuid();
                    string nomeFoto = veterinarios.NumCedulaProf + g.ToString();
                    string extensaoFoto = Path.GetExtension(fotoVet.FileName).ToLower();
                    nomeFoto += extensaoFoto;
                    //atribuir ao vet o nome da sua foto
                    veterinarios.Fotografia = nomeFoto;
                }
            }

            //avaliar se os dados
            if (ModelState.IsValid) {
                try {
                    //adicionar os dados á BD
                    _context.Add(veterinarios);
                    //consolidar esses dados (commit)
                    await _context.SaveChangesAsync();
                } catch (Exception) {
                    //é da nossa responsabilidade!!! tratarmos da exceção

                    //registar no disco rígido do servidor todos os dados da operação
                    //      - data + hora
                    //      - nome do utilizador
                    //      - nome do controller + método
                    //      - dados do erro (ex)
                    //      - outros dados considerados úteis
                    //eventualmente, tentar guardar na (numa) base de dados os dados do erro
                    //eventualmente, notificar o Administrador da app do erro

                    //no nosso caso,
                    //criar uma mensagem de erro
                    ModelState.AddModelError("", "Ocurreu um erro com a operação de guardar os dados do veterinário (" + veterinarios.Nome + ")");
                    //devolver o controlo á View
                    return View(veterinarios);
                }
                //++++++++++++++++++++++++++++++++++++++++
                //concretizar a ação de guardar o ficheiro
                //++++++++++++++++++++++++++++++++++++++++
                if (fotoVet != null) {
                    //onde o ficheiro var ser guardado?
                    string nomeLocalizacaoFicheiro = _webHostEnvironment.WebRootPath;
                    nomeLocalizacaoFicheiro = Path.Combine(nomeLocalizacaoFicheiro, "Fotos");
                    //avaliar se a pasta 'Fotos? existe
                    if (!Directory.Exists(nomeLocalizacaoFicheiro)) {
                        Directory.CreateDirectory(nomeLocalizacaoFicheiro);
                    }
                    //nome do documento a guardar
                    string nomeDaFoto = Path.Combine(nomeLocalizacaoFicheiro, veterinarios.Fotografia);
                    //criar o objeto que vai manipular o ficheiro
                    using var stream = new FileStream(nomeDaFoto, FileMode.Create);
                    //guardar no disco rígido
                    await fotoVet.CopyToAsync(stream);

                    //devolver o controlo da app á View
                    return RedirectToAction(nameof(Index));
                }
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
        public async Task<IActionResult> DeleteConfirmed(int id) {

            try {
                var veterinarios = await _context.Veterinarios.FindAsync(id);
                _context.Veterinarios.Remove(veterinarios);
                await _context.SaveChangesAsync();

                //remover o ficheiro com a foto do Veterinario
                //se a foto NÂO FOR a 'noVet.png'

            } catch (Exception) {
                //throw;
                //não esquecer, tratar da exceção

            }

            return RedirectToAction(nameof(Index));
        }

        private bool VeterinariosExists(int id) {
            return _context.Veterinarios.Any(e => e.Id == id);
        }
    }
}
