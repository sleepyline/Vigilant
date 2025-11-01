using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization;

namespace VigiLant.Controllers
{
    [Authorize]
    public class RiscosController : Controller
    {
        private readonly IRiscoRepository _riscoRepository;

        public RiscosController(IRiscoRepository riscoRepository)
        {
            _riscoRepository = riscoRepository;
        }

        public IActionResult Index()
        {
            var riscos = _riscoRepository.GetAll();
            return View(riscos);
        }
        
    }
}