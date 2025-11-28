using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization;

namespace VigiLant.Controllers
{
    [Authorize]
    public class ColaboradoresController : Controller
    {
        private readonly IColaboradorRepository _colaboradorRepository;

        public ColaboradoresController(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        // GET: /Colaboradores/Index
        public IActionResult Index()
        {
            var colaboradores = _colaboradorRepository.GetAll();
            return View(colaboradores);
        }
        
        // GET: /Colaboradores/Create
        public IActionResult Create()
        {
            // Inicializa o modelo com a data de hoje para evitar NullReferenceException
            return View(new Colaborador { DataAdmissao = DateTime.Today }); 
        }

        // POST: /Colaboradores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                _colaboradorRepository.Add(colaborador);
                return RedirectToAction(nameof(Index));
            }
            return View(colaborador);
        }

        // GET: /Colaboradores/Details/5
        public IActionResult Details(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            return View(colaborador);
        }

        // GET: /Colaboradores/Edit/5
        public IActionResult Edit(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            return View(colaborador);
        }

        // POST: /Colaboradores/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _colaboradorRepository.Update(colaborador);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception) 
                {
                    // Tratamento de erro, pode ser mais detalhado
                    return View(colaborador);
                }
            }
            return View(colaborador);
        }

        // GET: /Colaboradores/DeleteConfirmation/5
        public IActionResult DeleteConfirmation(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            return View(colaborador);
        }

        // POST: /Colaboradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _colaboradorRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}