// VigiLant.Repository/EquipamentoRepository.cs
using VigiLant.Contratos;
using VigiLant.Models;
using VigiLant.Data; // Assumindo que seu DbContext está em VigiLant.Data
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace VigiLant.Repository
{
    // A implementação usa IEquipamentoRepository (definida anteriormente)
    public class EquipamentoRepository : IEquipamentoRepository
    {
        // Usa o DbContext injetado
        private readonly BancoCtx _context; 

        public EquipamentoRepository(BancoCtx context)
        {
            _context = context;
        }

        // --- LÓGICA DE GERAÇÃO MANUAL DE ID (Seguindo o padrão do RelatorioRepository) ---
        private int GetNextAvailableId()
        {
            // Nota: Esta abordagem de gerar IDs manualmente e procurar o próximo ID livre 
            // é incomum e pode ter problemas de concorrência. O padrão é usar IDENTITY no BD.
            var existingIds = _context.Equipamentos // Assumindo que o DbSet é 'Equipamentos'
                                      .Select(e => e.Id)
                                      .OrderBy(id => id)
                                      .ToList();

            if (!existingIds.Any())
            {
                return 1;
            }

            int nextId = 1;
            
            foreach (var id in existingIds)
            {
                if (id > nextId)
                {
                    return nextId; 
                }
                nextId = id + 1;
            }

            return nextId;
        }
        // ---------------------------------------------------------------------------------


        public IEnumerable<Equipamento> GetAll()
        {
            // Retorna todos os equipamentos.
            return _context.Equipamentos.ToList();
        }

        public Equipamento GetById(int id)
        {
            // Busca um equipamento pelo ID.
            return _context.Equipamentos.Find(id);
        }

        public void Add(Equipamento equipamento)
        {
            // 1. Gera o próximo ID manualmente antes de adicionar.
            equipamento.Id = GetNextAvailableId(); 
            
            // 2. Adiciona ao contexto e salva as mudanças.
            _context.Equipamentos.Add(equipamento);
            _context.SaveChanges(); 
        }

        public void Update(Equipamento equipamento)
        {
            // 1. Marca o estado da entidade como modificado.
            _context.Entry(equipamento).State = EntityState.Modified;
            
            // 2. Salva as mudanças.
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            // 1. Busca o equipamento.
            var equipamento = GetById(id);
            
            if (equipamento != null)
            {
                // 2. Remove do DbSet e salva as mudanças.
                _context.Equipamentos.Remove(equipamento);
                _context.SaveChanges(); 
            }
        }
    }
}