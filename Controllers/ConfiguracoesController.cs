using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;

namespace VigiLant.Controllers
{
    [Authorize]
    public class ConfiguracoesController : Controller
    {
        private readonly IMqttConfigurationRepository _configRepository;

        public ConfiguracoesController(IMqttConfigurationRepository configRepository)
        {
            _configRepository = configRepository;
        }

        // GET: /Configuracoes/Index
        [HttpGet]
        public IActionResult Index()
        {
            var config = _configRepository.GetConfiguration();
            return View(config);
        }

        // POST: /Configuracoes/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(MqttConfiguration config)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _configRepository.SaveConfiguration(config);
                    TempData["SuccessMessage"] = "Configurações do Broker MQTT salvas com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar: {ex.Message}");
                }
            }
            return View(config);
        }
    }
}