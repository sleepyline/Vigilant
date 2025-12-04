using Microsoft.AspNetCore.Mvc;
using VigiLant.Contratos;
using VigiLant.Data;
using VigiLant.Models.ViewModels;
using System.Linq;
using VigiLant.Models.Enum;
using Microsoft.AspNetCore.Authorization;

namespace VigiLant.Controllers
{
    [Authorize]
    public class IAController : Controller
    {
        private readonly IRiscoRepository _riscoRepository;
        private readonly BancoCtx _context;

        public IAController(IRiscoRepository riscoRepository, BancoCtx context)
        {
            _riscoRepository = riscoRepository;
            _context = context;
        }

        // Action que será chamada via AJAX ao clicar no botão "IA"
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AnaliseRiscos()
        {
            // 1. Coleta e Análise de Dados
            var riscos = _riscoRepository.GetAll().ToList();
            // Acessa o DbSet<Equipamento> do seu DbContext
            var equipamentos = _context.Equipamentos.ToList();

            var model = new IAAnalysisModel
            {
                RiscosAnalisados = riscos,
                TotalRiscos = riscos.Count,
                TotalEquipamentos = equipamentos.Count
            };

            // 2. SIMULAÇÃO da Lógica de IA para Soluções
            model.SolucoesSugeridas.Add($"Implementar treinamento de segurança para {riscos.Count(r => r.TipoRisco == TipoRisco.Fisico)} riscos de categoria Humana.");
            model.SolucoesSugeridas.Add("Revisar manutenção preventiva em 30% dos Equipamentos de alto risco no próximo mês.");

            // 3. SIMULAÇÃO da Lógica de IA para Previsões
            // (A lógica abaixo é um placeholder para o seu motor de IA real)
            var equipamentosComRisco = equipamentos.Take(2).Select(e => $"Equipamento ID {e.Id}").ToList();
            model.PrevisoesDeRisco.Add($"Risco de falha de segurança (Nível Alto) detectado nos seguintes equipamentos: {string.Join(", ", equipamentosComRisco)}.");
            model.PrevisoesDeRisco.Add($"Aumento na gravidade dos riscos físicos se a manutenção dos equipamentos for negligenciada.");

            // 4. Retorna a Partial View com o modelo preenchido
            return PartialView("_AnaliseIAPartial", model);
        }
    }
}