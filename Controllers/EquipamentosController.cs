using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Models;

namespace VigiLant.Controllers;

[Authorize]
public class EquipamentosController : Controller
{
    private readonly IEquipamentoRepository _equipamentoRepository;

    public EquipamentosController(IEquipamentoRepository equipamentoRepository)
    {
        _equipamentoRepository = equipamentoRepository;
    }

    public IActionResult Index()
    {
        var equipamentos = _equipamentoRepository.GetAll();
        return View(equipamentos);
    }
}

