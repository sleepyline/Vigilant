using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using VigiLant.Services;
using VigiLant.Repository; // Presumindo que você terá um serviço para MQTT

namespace VigiLant.Controllers
{
    [Authorize]
    public class EquipamentosController : Controller
    {
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly IMqttService _mqttService; // Interface para o serviço MQTT

        // O repositório deve ser criado em VigiLant.Contratos
        public EquipamentosController(IEquipamentoRepository equipamentoRepository, IMqttService mqttService)
        {
            _equipamentoRepository = equipamentoRepository;
            _mqttService = mqttService;
        }

        // Helper para verificar se a requisição é AJAX
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

        // GET: /Equipamentos/Create
        public IActionResult Create()
        {
            var novoEquipamento = new Equipamento
            {
                Status = "PendenteConexao"
            };

            if (IsAjaxRequest())
            {
                return PartialView("_CreateEquipamentoPartial", novoEquipamento);
            }
            return View(novoEquipamento);
        }

        // POST: /Equipamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Equipamento equipamento)
        {
            // Garante o status inicial de conexão
            equipamento.Status = "PendenteConexao";
            equipamento.DataUltimaManutencao = DateTime.Now; // Usado para inicializar a data

            if (ModelState.IsValid)
            {
                // 1. Salva o equipamento no banco de dados
                _equipamentoRepository.Add(equipamento);

                try
                {
                    // 2. Envia a mensagem de "SeConectar" via MQTT
                    // O TopicoResposta será usado pelo serviço de background
                    await _mqttService.PublicarMensagemAsync(
                        $"{equipamento.Topico}/comando", // Ex: sensor/temp/comando
                        "SeConectar"
                    );

                    // 3. O sucesso é imediato (a resposta do broker é assíncrona)
                    if (IsAjaxRequest())
                    {
                        return Ok(); // Sucesso
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Trata erro de comunicação MQTT
                    ModelState.AddModelError(string.Empty, "Erro ao tentar enviar o comando 'SeConectar' ao broker.");
                    // Define o Status para indicar falha na comunicação inicial
                    equipamento.Status = "ErroInicial";
                }
            }

            // SE VALIDAÇÃO FALHAR OU ERRO MQTT
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400;
                return PartialView("_CreateEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // GET: /Equipamentos/Details/5
        public IActionResult Details(int id)
        {
            var equipamento = _equipamentoRepository.GetById(id);
            if (equipamento == null)
            {
                if (IsAjaxRequest()) { Response.StatusCode = 404; return Content("Equipamento não encontrado."); }
                return NotFound();
            }

            if (IsAjaxRequest())
            {
                return PartialView("_DetailsEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // GET: /Equipamentos/Edit/5
        public IActionResult Edit(int id)
        {
            var equipamento = _equipamentoRepository.GetById(id);
            if (equipamento == null)
            {
                if (IsAjaxRequest()) { Response.StatusCode = 404; return Content("Equipamento não encontrado."); }
                return NotFound();
            }

            if (IsAjaxRequest())
            {
                return PartialView("_EditEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // POST: /Equipamentos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Equipamento equipamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _equipamentoRepository.Update(equipamento);

                    if (IsAjaxRequest())
                    {
                        return Ok();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar as alterações.");

                    if (IsAjaxRequest())
                    {
                        Response.StatusCode = 400;
                        return PartialView("_EditEquipamentoPartial", equipamento);
                    }
                    return View(equipamento);
                }
            }

            // SE VALIDAÇÃO FALHAR
            if (IsAjaxRequest())
            {
                Response.StatusCode = 400;
                return PartialView("_EditEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

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
                return PartialView("_DeleteEquipamentoPartial", equipamento);
            }
            return View(equipamento);
        }

        // POST: /Equipamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _equipamentoRepository.Delete(id);

            if (IsAjaxRequest())
            {
                return Ok();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}