using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;

namespace VigiLant.Controllers;

public class EquipamentosController : Controller
{
    private static List<Equipamento> equipamentos = new List<Equipamento>
        {
            new Equipamento { Id = 1, Nome = "Extintor A-001", Tipo = "Extintor de Incêndio", Localizacao = "Sala 101", Status = "Ativo", DataManutencao = DateTime.Now.AddDays(30) },
            new Equipamento { Id = 2, Nome = "Máquina de Corte XZ-500", Tipo = "Equipamento Industrial", Localizacao = "Galpão 2", Status = "Manutenção", DataManutencao = DateTime.Now.AddDays(-2) },
            new Equipamento { Id = 3, Nome = "EPI - Capacete C-50", Tipo = "Equipamento de Proteção", Localizacao = "Almoxarifado", Status = "Ativo", DataManutencao = DateTime.Now.AddDays(60) },
            new Equipamento { Id = 4, Nome = "Detector de Fumaça DF-200", Tipo = "Sistema de Segurança", Localizacao = "Corredor Principal", Status = "Ativo", DataManutencao = DateTime.Now.AddDays(15) },
            new Equipamento { Id = 5, Nome = "Empilhadeira EMP-7", Tipo = "Veículo Industrial", Localizacao = "Depósito", Status = "Ativo", DataManutencao = DateTime.Now.AddDays(45) }
        };

    public IActionResult Index()
    {
        return View(equipamentos);
    }
}

