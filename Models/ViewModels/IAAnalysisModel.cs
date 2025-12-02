using System.Collections.Generic;
using VigiLant.Models;

namespace VigiLant.Models.ViewModels
{
    public class IAAnalysisModel
    {
        // Lista de riscos que foram submetidos à análise (opcional)
        public List<Risco> RiscosAnalisados { get; set; } = new List<Risco>();
        
        // Resultados da "IA"
        public List<string> SolucoesSugeridas { get; set; } = new List<string>();
        public List<string> PrevisoesDeRisco { get; set; } = new List<string>();
        
        // Dados estatísticos simples
        public int TotalRiscos { get; set; }
        public int TotalEquipamentos { get; set; }
    }
}