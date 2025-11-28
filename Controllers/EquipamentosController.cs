using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Services;
using System.Threading.Tasks;
using System;
using System.Linq;
using VigiLant.Repositories; // Mantenha este using

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

        // ====================================================================
        // 1. LISTAGEM (INDEX)
        // ====================================================================

        public async Task<IActionResult> Index()
        {
            var equipamentos = await _repo.GetAllAsync();
            return View(equipamentos);
        }

        // ====================================================================
        // 2. CONECTAR NOVO EQUIPAMENTO (GET)
        // ====================================================================
        public IActionResult Conectar()
        {
            return View();
        }

        // ====================================================================
        // 3. SALVAR EQUIPAMENTO (POST - AJAX)
        // ====================================================================

        [HttpPost]
        public async Task<IActionResult> SalvarEquipamento([FromBody] Equipamento equipamento)
        {
            if (equipamento == null || string.IsNullOrEmpty(equipamento.Nome) || string.IsNullOrEmpty(equipamento.TipoSensor))
            {
                return BadRequest(new { 
                    success = false, 
                    message = "Dados do equipamento inválidos ou incompletos." 
                });
            }

            try
            {
                equipamento.Status = "Ativo";
                equipamento.DataUltimaManutencao = DateTime.Now;
                
                await _repo.AddAsync(equipamento);

                // Opcional: Registrar a nova conexão no serviço MQTT
                // await _mqtt.RegisterEquipmentTopic(equipamento.Topico); 

                return Json(new {
                    success = true,
                    message = $"Equipamento '{equipamento.Nome}' salvo com sucesso.",
                    id = equipamento.Id,
                    nome = equipamento.Nome
                });
            }
            catch (Exception ex)
            {
                // Considere usar um logger real aqui
                return StatusCode(500, new { 
                    success = false, 
                    message = $"Erro interno ao salvar o equipamento: {ex.Message}" 
                });
            }
        }
        
    }
}