using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;

namespace VigiLant.Controllers;

public class ColaboradoresController : Controller
{
    private static List<Colaborador> colaboradores = new List<Colaborador>
        {
            new Colaborador { Id = 1, Nome = "João Silva", Cargo = "Engenheiro de Segurança", Departamento = "Segurança do Trabalho", Email = "joao.silva@empresa.com", Telefone = "(11) 98765-4321", DataAdmissao = DateTime.Now.AddYears(-3) },
            new Colaborador { Id = 2, Nome = "Maria Santos", Cargo = "Técnica de Segurança", Departamento = "Segurança do Trabalho", Email = "maria.santos@empresa.com", Telefone = "(11) 98765-1234", DataAdmissao = DateTime.Now.AddYears(-2) },
            new Colaborador { Id = 3, Nome = "Pedro Oliveira", Cargo = "Supervisor de Produção", Departamento = "Produção", Email = "pedro.oliveira@empresa.com", Telefone = "(11) 98765-5678", DataAdmissao = DateTime.Now.AddYears(-5) },
            new Colaborador { Id = 4, Nome = "Ana Costa", Cargo = "Coordenadora HSE", Departamento = "Segurança do Trabalho", Email = "ana.costa@empresa.com", Telefone = "(11) 98765-9012", DataAdmissao = DateTime.Now.AddYears(-4) },
            new Colaborador { Id = 5, Nome = "Carlos Mendes", Cargo = "Operador de Máquinas", Departamento = "Produção", Email = "carlos.mendes@empresa.com", Telefone = "(11) 98765-3456", DataAdmissao = DateTime.Now.AddYears(-1) }
        };

    public IActionResult Index()
    {
        return View(colaboradores);
    }
}

