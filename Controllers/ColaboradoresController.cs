using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;
using VigiLant.Contratos;
using Microsoft.AspNetCore.Authorization; 

namespace VigiLant.Controllers;

[Authorize]
public class ColaboradoresController : Controller
{
    private readonly IColaboradorRepository _colaboradorRepository;

    public ColaboradoresController(IColaboradorRepository colaboradorRepository)
    {
        _colaboradorRepository = colaboradorRepository;
    }

    public IActionResult Index()
    {
        var colaboradores = _colaboradorRepository.GetAll();
        return View(colaboradores);
    }
    
}