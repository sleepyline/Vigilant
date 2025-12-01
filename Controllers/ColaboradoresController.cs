using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization;
using System; 

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

        // Helper para verificar se a requisição é AJAX
        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest"; 
        }

        // GET: /Colaboradores/Index
        public IActionResult Index()
        {
            var colaboradores = _colaboradorRepository.GetAll();
            return View(colaboradores);
        }

        // GET: /Colaboradores/Create -> Retorna a Partial
        public IActionResult Create()
        {
            var novoColaborador = new Colaborador { DataAdmissao = DateTime.Today };
            
            if (IsAjaxRequest())
            {
                return PartialView("_CreateColaboradorPartial", novoColaborador); 
            }
            return View(novoColaborador);
        }

        // POST: /Colaboradores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                _colaboradorRepository.Add(colaborador);
                
                if (IsAjaxRequest()) { return Ok(); } // Sucesso AJAX
                return RedirectToAction(nameof(Index));
            }
            
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400; // Erro de Validação
                return PartialView("_CreateColaboradorPartial", colaborador);
            }
            return View(colaborador);
        }

        // GET: /Colaboradores/Details/5 -> Retorna a Partial
        public IActionResult Details(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            if (colaborador == null) { return NotFound(); }

            if (IsAjaxRequest())
            {
                return PartialView("_DetailsColaboradorPartial", colaborador); 
            }
            return View(colaborador);
        }

        // GET: /Colaboradores/Edit/5 -> Retorna a Partial
        public IActionResult Edit(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            if (colaborador == null) { return NotFound(); }
            
            if (IsAjaxRequest())
            {
                return PartialView("_EditColaboradorPartial", colaborador); 
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
                _colaboradorRepository.Update(colaborador);
                if (IsAjaxRequest()) { return Ok(); } // Sucesso AJAX
                return RedirectToAction(nameof(Index));
            }
            
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400; 
                return PartialView("_EditColaboradorPartial", colaborador); // Retorna a Partial com erros
            }
            return View(colaborador);
        }

        // GET: /Colaboradores/DeleteConfirmation/5 -> Retorna a Partial
        public IActionResult DeleteConfirmation(int id)
        {
            var colaborador = _colaboradorRepository.GetById(id);
            if (colaborador == null) { return NotFound(); }
            
            if (IsAjaxRequest())
            {
                return PartialView("_DeleteColaboradorPartial", colaborador); 
            }
            return View(colaborador);
        }

        // POST: /Colaboradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _colaboradorRepository.Delete(id);
            
            if (IsAjaxRequest()) { return Ok(); } // Sucesso AJAX
            return RedirectToAction(nameof(Index));
        }
    }
}