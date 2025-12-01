using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using VigiLant.Repositories;

namespace VigiLant.Controllers
{
    [Authorize]
    public class EquipamentosController : Controller
    {
        private readonly IEquipamentoRepository _repo;
        private readonly IMqttService _mqtt;

        public EquipamentosController(IEquipamentoRepository repo, IMqttService mqtt)
        {
            _repo = repo;
            _mqtt = mqtt;
        }

        // Helper para verificar se a requisição é AJAX
        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        // GET: /Equipamentos/Index
        public async Task<IActionResult> Index()
        {
            var equipamentos = await _repo.GetAllAsync();
            return View(equipamentos);
        }
        
        public IActionResult Conectar()
        {
            var novoEquipamento = new Equipamento { DataUltimaManutencao = DateTime.Now };

            if (IsAjaxRequest())
            {
                return PartialView("_ConectarEquipamentoPartial", novoEquipamento);
            }
            return View(novoEquipamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Conectar(Equipamento equipamento)
        {
            if (ModelState.IsValid)
            {
                equipamento.Status = equipamento.Status ?? "Ativo"; // Define um valor padrão se for null
                equipamento.DataUltimaManutencao = DateTime.Now; // Garante a data atual na criação

                await _repo.AddAsync(equipamento);
                
                if (IsAjaxRequest()) { return Ok(); } // Sucesso AJAX (HTTP 200 OK)
                return RedirectToAction(nameof(Index));
            }
            
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400; // Erro de Validação (HTTP 400 Bad Request)
                return PartialView("_ConectarEquipamentoPartial", equipamento); // Retorna a Partial com erros
            }
            return View(equipamento);
        }

        // GET: /Equipamentos/Details/5 -> Retorna a Partial
        public async Task<IActionResult> Details(int id)
        {
            var equipamento = await _repo.GetByIdAsync(id);
            if (equipamento == null) { return NotFound(); }

            if (IsAjaxRequest())
            {
                return PartialView("_DetailsEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // GET: /Equipamentos/Edit/5 -> Retorna a Partial
        public async Task<IActionResult> Edit(int id)
        {
            var equipamento = await _repo.GetByIdAsync(id);
            if (equipamento == null) { return NotFound(); }
            
            if (IsAjaxRequest())
            {
                return PartialView("_EditEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // POST: /Equipamentos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Equipamento equipamento)
        {
            if (ModelState.IsValid)
            {
                await _repo.UpdateAsync(equipamento);
                if (IsAjaxRequest()) { return Ok(); } // Sucesso AJAX
                return RedirectToAction(nameof(Index));
            }
            
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400; 
                return PartialView("_EditEquipamentoPartial", equipamento); // Retorna a Partial com erros
            }
            return View(equipamento);
        }

        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var equipamento = await _repo.GetByIdAsync(id);
            if (equipamento == null) { return NotFound(); }
            
            if (IsAjaxRequest())
            {
                return PartialView("_DeleteEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // POST: /Equipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            
            if (IsAjaxRequest()) { return Ok(); } // Sucesso AJAX
            return RedirectToAction(nameof(Index));
        }
    }
}