using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos; 

namespace VigiLant.Controllers
{
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
        
        // Você adicionaria outros métodos como Details, Create, Edit, Delete aqui,
        // usando os métodos do _riscoRepository (GetById, Add, Update, Delete).
    }
}