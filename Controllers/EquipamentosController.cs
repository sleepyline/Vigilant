// Controllers/EquipamentosController.cs
// (CÓDIGO COMPLETO E AJUSTADO)

using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization;
using VigiLant.Models.Enum;
using System;
using System.Threading.Tasks;

namespace VigiLant.Controllers
{
    [Authorize]
    public class EquipamentosController : Controller
    {
        private readonly IEquipamentoRepository _equipamentoRepository;

        public EquipamentosController(IEquipamentoRepository equipamentoRepository)
        {
            _equipamentoRepository = equipamentoRepository;
        }
        
        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest"; 
        }

        // GET: /Equipamentos/Index
        public IActionResult Index()
        {
            var equipamentos = _equipamentoRepository.GetAll();
            return View(equipamentos);
        }
        
        // GET: /Equipamentos/Conectar
        public IActionResult Conectar()
        {
            if (IsAjaxRequest())
            {
                return PartialView("_ConectarEquipamentoPartial", new Equipamento());
            }
            return View(new Equipamento());
        }

        // POST: /Equipamentos/Conectar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Conectar(string identificador)
        {
            if (string.IsNullOrWhiteSpace(identificador))
            {
                ModelState.AddModelError("identificador", "O identificador do equipamento é obrigatório para conectar.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var novoEquipamento = _equipamentoRepository.Conectar(identificador);

                    if (IsAjaxRequest())
                    {
                        return Ok(new { success = true, id = novoEquipamento.Id }); 
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message); 
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Erro ao tentar cadastrar: {ex.Message}");
                }
            }
            
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400; 
                return PartialView("_ConectarEquipamentoPartial", new Equipamento() { IdentificadorBroker = identificador });
            }
            return View(new Equipamento());
        }

        // ---------------------------------------------
        // NOVO: VISUALIZAR DETALHES (Read)
        // ---------------------------------------------

        // GET: /Equipamentos/Details/5
        public IActionResult Details(int id)
        {
            var equipamento = _equipamentoRepository.GetById(id);
            if (equipamento == null)
            {
                if (IsAjaxRequest())
                {
                    Response.StatusCode = 404;
                    return Content("Equipamento não encontrado.");
                }
                return NotFound();
            }

            if (IsAjaxRequest())
            {
                // Retorna a view parcial de detalhes
                return PartialView("_DetailsEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // GET: /Equipamentos/Monitorar/5
        public IActionResult Monitorar(int id)
        {
            var equipamento = _equipamentoRepository.GetById(id);
            if (equipamento == null)
            {
                if (IsAjaxRequest())
                {
                    Response.StatusCode = 404;
                    return Content("Equipamento não encontrado.");
                }
                return NotFound();
            }

            if (IsAjaxRequest())
            {
                return PartialView("_MonitorarEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }
        
        // GET: /Equipamentos/GetRealTimeData/5
        [HttpGet]
        public IActionResult GetRealTimeData(int id)
        {
            var equipamento = _equipamentoRepository.GetById(id);
            if (equipamento == null)
            {
                return NotFound();
            }

            // Retorna um JSON com os dados ATUALIZADOS DO BANCO
            return Json(new 
            {
                nome = equipamento.Nome,
                localizacao = equipamento.Localizacao,
                status = equipamento.Status.ToString(),
                tipoSensor = equipamento.TipoSensor.ToString(),
                ultimaAtualizacao = equipamento.UltimaAtualizacao.ToString("dd/MM/yyyy HH:mm:ss")
            });
        }

        // ---------------------------------------------
        // NOVO: EXCLUIR/DESCONECTAR (Delete)
        // ---------------------------------------------

        // GET: /Equipamentos/DeleteConfirmation/5 
        public IActionResult DeleteConfirmation(int id)
        {
            var equipamento = _equipamentoRepository.GetById(id);
            if (equipamento == null)
            {
                 if (IsAjaxRequest())
                {
                    Response.StatusCode = 404;
                    return Content("Equipamento não encontrado para exclusão.");
                }
                return NotFound();
            }
            
            if (IsAjaxRequest())
            {
                // Retorna a view parcial de confirmação de exclusão
                return PartialView("_DeleteEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // POST: /Equipamentos/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _equipamentoRepository.Delete(id);
            
            // Opcional: Se necessário, enviar uma mensagem de "desativação" ao Broker aqui.

            if (IsAjaxRequest())
            {
                // Retorna Status 200/OK para o JavaScript (Sucesso)
                return Ok(); 
            }
            return RedirectToAction(nameof(Index));
        }
    }
}