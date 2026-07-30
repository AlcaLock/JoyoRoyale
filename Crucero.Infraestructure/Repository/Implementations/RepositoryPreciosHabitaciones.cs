using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RepositoryPrecioHabitacion : IRepositoryPreciosHabitaciones
{
    private readonly JoyoRoyaleContext _context;

    public RepositoryPrecioHabitacion(JoyoRoyaleContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(PreciosHabitaciones entity)
    {
        _context.PreciosHabitaciones.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id; 
    }
    public async Task AddRangeAsync(List<PreciosHabitaciones> entidades)
    {
        await _context.PreciosHabitaciones.AddRangeAsync(entidades);
        await _context.SaveChangesAsync();  // Guardar todos los cambios en la base de datos
    }

    public async Task UpdateAsync(PreciosHabitaciones entity)
    {
        _context.PreciosHabitaciones.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.PreciosHabitaciones.FindAsync(id);
        if (entity != null)
        {
            _context.PreciosHabitaciones.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<decimal> GetPrecioByHabitacionAndFechaAsync(int habitacionId, int fechaCruceroId)
    {
        return await _context.PreciosHabitaciones
            .Where(ph => ph.HabitacionId == habitacionId && ph.FechaCruceroId == fechaCruceroId)
            .Select(ph => ph.Precio)
            .FirstOrDefaultAsync();
    }



    public async Task<PreciosHabitaciones> FindByIdAsync(int id)
    {
        return await _context.PreciosHabitaciones.FindAsync(id);
    }

    public async Task<ICollection<PreciosHabitaciones>> ListAsync()
    {
        return await _context.PreciosHabitaciones.ToListAsync();
    }
}

