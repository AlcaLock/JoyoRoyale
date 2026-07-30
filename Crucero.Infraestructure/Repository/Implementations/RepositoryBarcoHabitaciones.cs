
using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryBarcoHabitaciones : IRepositoryBarcoHabitaciones
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryBarcoHabitaciones(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(List<BarcoHabitaciones> entidades)
        {
            await _context.BarcoHabitaciones.AddRangeAsync(entidades);
            await _context.SaveChangesAsync();
        }


        public async Task<ICollection<BarcoHabitaciones>> ListAsync()
        {
            return await _context.BarcoHabitaciones
  
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BarcoHabitaciones> FindByIdAsync(int id)
        {
            return await _context.BarcoHabitaciones

                .FirstOrDefaultAsync(bh => bh.Id == id);
        }

        public async Task<int> AddAsync(BarcoHabitaciones entity)
        {
            _context.BarcoHabitaciones.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(BarcoHabitaciones entity)
        {
            _context.BarcoHabitaciones.Update(entity);
            await _context.SaveChangesAsync();
        }

        // RepositoryBarcoHabitaciones.cs

        public async Task DeleteByBarcoIdAsync(int barcoId)
        {
            // Obtener las asociaciones de habitaciones para el barco
            var asociaciones = await _context.BarcoHabitaciones
                                             .Where(bh => bh.BarcoId == barcoId)
                                             .ToListAsync();

            // Eliminar todas las asociaciones encontradas
            _context.BarcoHabitaciones.RemoveRange(asociaciones);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<BarcoHabitaciones>> ListByBarcoIdAsync(int barcoId)
        {
            return await _context.BarcoHabitaciones
                                 .Where(bh => bh.BarcoId == barcoId)
                                 .ToListAsync();
        }


        public async Task<ICollection<BarcoHabitaciones>> FindByHabitacionIdAsync(int habitacionId)
        {
            return await _context.BarcoHabitaciones
                .Where(bh => bh.HabitacionId == habitacionId)

                .ToListAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<BarcoHabitaciones>> FindByBarcoIdAsync(int barcoId)
        {
            return await _context.BarcoHabitaciones
                                 .Where(bh => bh.BarcoId == barcoId)
                                 .Include(bh => bh.Habitacion) // Si necesitas detalles de la habitación
                                 .ToListAsync();
        }

    }
}