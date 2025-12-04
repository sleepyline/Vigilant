// Controllers/ConfiguracoesController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;

namespace VigiLant.Controllers
{
    [Authorize]
    public class ConfiguracoesController : Controller
    {
        private readonly IAppConfigRepository _configRepository;

        public ConfiguracoesController(IAppConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        // GET: /Configuracoes/Index
        public IActionResult Index()
        {
            var config = _configRepository.GetConfig();
            return View(config);
        }

        // POST: /Configuracoes/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AppConfig config)
        {
            config.Id = 1;

            if (ModelState.IsValid)
            {
                _configRepository.UpdateConfig(config);
                ViewBag.SuccessMessage = "Configurações salvas com sucesso!";
                return View(config);
            }

            // Se a validação falhar, retorna a view com os erros
            return View(config);
        }
    }
}