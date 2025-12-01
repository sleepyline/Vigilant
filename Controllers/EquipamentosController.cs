// Controllers/EquipamentosController.cs
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;
using System.Threading.Tasks;
using System;
using System.Linq;
using VigiLant.Repositories;

namespace VigiLant.Controllers
{
    public class EquipamentosController : Controller
    {
        private readonly IEquipamentoRepository _repo;
        private readonly IMqttService _mqtt;

        public EquipamentosController(IEquipamentoRepository repo, IMqttService mqtt)
        {
            _repo = repo;
            _mqtt = mqtt;
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public async Task<IActionResult> Index()
        {
            var equipamentos = await _repo.GetAllAsync();

            return View(equipamentos);
        }

        public IActionResult Conectar()
        {
            var novoEquipamento = new Equipamento
            {
                DataUltimaManutencao = DateTime.Now,
                Porta = 1883 // Define o valor padrão para a Partial View
            };

            if (IsAjaxRequest())
            {
                return PartialView("_ConectarEquipamentosPartial", novoEquipamento);
            }
            return View(novoEquipamento);
        }


        // POST: /Equipamentos/Conectar (Ação Principal)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Conectar(Equipamento equipamento)
        {
            if (!ModelState.IsValid)
            {
                if (IsAjaxRequest())
                {
                    Response.StatusCode = 400;
                    return PartialView("_ConectarEquipamentosPartial", equipamento);
                }
                return View(equipamento);
            }

            try
            {
                // 1. Definir o Tópico de Resposta (Convenção: Tópico Medição + /status)
                string baseTopic = equipamento.Topico.TrimEnd('/');
                equipamento.TopicoResposta = $"{baseTopic}/status";
                equipamento.Status = "Aguardando Conexão";

                // 2. Cadastrar o Equipamento Provisoriamente no banco de dados.
                await _repo.AddAsync(equipamento);

                // 3. Assinar os tópicos no broker para escutar a medição e a resposta
                await _mqtt.AssinarTopicoAsync(equipamento.Topico);
                await _mqtt.AssinarTopicoAsync(equipamento.TopicoResposta);

                // 4. Publicar a mensagem "Conectando" (Gatilho para o dispositivo IoT)
                await _mqtt.PublicarAsync(equipamento.Topico, "Conectando");

                // 5. Sucesso (a confirmação final será feita pelo MqttHostedService)
                if (IsAjaxRequest())
                {
                    return Ok(new
                    {
                        success = true,
                        message = $"Tentativa de conexão iniciada. Aguardando resposta em: {equipamento.TopicoResposta}..."
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Erro ao processar a conexão: " + ex.Message);
                if (IsAjaxRequest())
                {
                    Response.StatusCode = 500;
                    return PartialView("_ConectarEquipamentosPartial", equipamento);
                }
                return View(equipamento);
            }
        }
    }
}