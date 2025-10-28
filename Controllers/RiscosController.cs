using Microsoft.AspNetCore.Mvc;
using VigiLant.Models;

namespace VigiLantMVC.Controllers
{
    public class RiscosController : Controller
    {
        private static List<Risco> riscos = new List<Risco>
        {
            new Risco { Id = 1, Nome = "Risco Elétrico", Descricao = "Instalação elétrica inadequada", NivelGravidade = "Alto", Status = "Identificado", DataIdentificacao = DateTime.Now.AddDays(-10) },
            new Risco { Id = 2, Nome = "Risco de Incêndio", Descricao = "Falta de extintores", NivelGravidade = "Crítico", Status = "Em Análise", DataIdentificacao = DateTime.Now.AddDays(-5) },
            new Risco { Id = 3, Nome = "Risco Químico", Descricao = "Armazenamento inadequado de produtos químicos", NivelGravidade = "Médio", Status = "Resolvido", DataIdentificacao = DateTime.Now.AddDays(-20) },
            new Risco { Id = 4, Nome = "Risco Ergonômico", Descricao = "Mobiliário inadequado", NivelGravidade = "Baixo", Status = "Identificado", DataIdentificacao = DateTime.Now.AddDays(-15) },
            new Risco { Id = 5, Nome = "Risco de Queda", Descricao = "Piso escorregadio", NivelGravidade = "Alto", Status = "Em Tratamento", DataIdentificacao = DateTime.Now.AddDays(-8) }
        };

        public IActionResult Index()
        {
            return View(riscos);
        }
    }
}
