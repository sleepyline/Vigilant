using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization;
using VigiLant.Models.Enum; // Certifique-se de adicionar este using para o Enum

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

        // GET: /Riscos/Index
        public IActionResult Index()
        {
            var riscos = _riscoRepository.GetAll();
            return View(riscos);
        }

        public IActionResult Create()
        {
            // Inicializa o objeto Risco com uma DataIdentificacao padrão (ex: hoje)
            // para garantir que o Model na View não seja nulo.
            var novoRisco = new Risco
            {
                DataIdentificacao = DateTime.Today // Define a data inicial
            };
            return View(novoRisco);
        }

        // POST: /Riscos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Risco risco)
        {
            // Garante que a DataIdentificacao não seja uma data inválida para a base de dados
            if (risco.DataIdentificacao == default(DateTime))
            {
                risco.DataIdentificacao = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                _riscoRepository.Add(risco); // Usa o método Add implementado no RiscoRepository
                return RedirectToAction(nameof(Index));
            }

            // Se houver erros de validação, retorna a View para o usuário corrigir
            return View(risco);
        }

        // GET: /Riscos/Details/5
        public IActionResult Details(int id)
        {
            var risco = _riscoRepository.GetById(id);
            if (risco == null)
            {
                return NotFound();
            }
            return View(risco);
        }

        // GET: /Riscos/Edit/5
        public IActionResult Edit(int id)
        {
            var risco = _riscoRepository.GetById(id);
            if (risco == null)
            {
                return NotFound();
            }
            // Você pode precisar passar listas de seleção (Ex: NivelSeveridade, Status) para a View aqui se não usar Tags Helpers de Enum diretamente.
            return View(risco);
        }

        // POST: /Riscos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Risco risco)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _riscoRepository.Update(risco);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception) // Catch de exceção mais específica se souber qual é (Ex: DbUpdateConcurrencyException)
                {
                    // Tratar erro ou redirecionar para uma página de erro
                    return View(risco);
                }
            }
            return View(risco);
        }

        // GET: /Riscos/DeleteConfirmation/5 (Exibe a mensagem de confirmação)
        public IActionResult DeleteConfirmation(int id)
        {
            var risco = _riscoRepository.GetById(id);
            if (risco == null)
            {
                return NotFound();
            }
            return View(risco);
        }

        // POST: /Riscos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _riscoRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}