using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Entities;

namespace Softools.Projetos.Services;

public class ProjetoService
{
    private readonly ProjetosDbContext _context;

    public ProjetoService(ProjetosDbContext context)
    {
        _context = context;
    }

    public async Task<List<Projeto>> BuscarProjetosTerminoProximo(int dias)
    {
        var hoje = DateOnly.FromDateTime(DateTime.Now);
        var limite = hoje.AddDays(dias);

        return await _context.Projetos
            .Where(p => p.DataFim >= hoje && p.DataFim <= limite)
            .ToListAsync();
    }

    public async Task GerarAlertaProjetosEmTermino()
    {
        var projetos = await BuscarProjetosTerminoProximo(7);

        foreach (var projeto in projetos )
        {
            Console.WriteLine($"Projeto {projeto.Nome} esta perto do Termino");
        }
        
    }
    
}