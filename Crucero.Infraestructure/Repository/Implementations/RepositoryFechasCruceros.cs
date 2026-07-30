using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RepositoryFechaCrucero : IRepositoryFechasCruceros
{
    private readonly JoyoRoyaleContext _context;

    public RepositoryFechaCrucero(JoyoRoyaleContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(FechasCruceros entity)
    {
        _context.FechasCruceros.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id; 
    }
    public async Task<List<FechasCruceros>> GetByCruceroIdAsync(int cruceroId)
    {
        return await _context.FechasCruceros
            .Where(f => f.CruceroId == cruceroId)
            .ToListAsync();
    }

    public async Task AddRangeAsync(List<FechasCruceros> entidades)
    {
        await _context.FechasCruceros.AddRangeAsync(entidades);
        await _context.SaveChangesAsync();  // Guardar todos los cambios en la base de datos
    }

    public async Task UpdateAsync(FechasCruceros entity)
    {
        _context.FechasCruceros.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.FechasCruceros.FindAsync(id);
        if (entity != null)
        {
            _context.FechasCruceros.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<FechasCruceros> FindByIdAsync(int id)
    {
        return await _context.FechasCruceros.FindAsync(id);
    }

    public async Task<ICollection<FechasCruceros>> ListAsync()
    {
        return await _context.FechasCruceros.ToListAsync();
    }
}
