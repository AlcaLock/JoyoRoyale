using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryCruceros : IRepositoryCruceros
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryCruceros(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Cruceros>> ListAsync()
        {
            var collection = await _context.Set<Cruceros>()
                                     .Include(x => x.Barco) // Incluir el barco en la consulta
                                     .AsNoTracking()
                                     .ToListAsync();
            return collection;
        }

        public async Task<Cruceros> FindByIdAsync(int id)
        {
            var crucero = await _context.Cruceros
                .Include(x => x.Barco)
                    .ThenInclude(b => b.BarcoHabitaciones) // Incluir las habitaciones del barco
                        .ThenInclude(bh => bh.Habitacion) // Incluir la información de la habitación
                .Include(x => x.Itinerarios.OrderBy(i => i.Dia)) // Ordena itinerarios por día
                    .ThenInclude(i => i.Puerto)
                .Include(x => x.FechasCruceros)
                    .ThenInclude(f => f.PreciosHabitaciones) // Incluye precios de habitaciones
                        .ThenInclude(p => p.Habitacion) // Incluye la habitación asociada a cada precio
                .FirstOrDefaultAsync(x => x.Id == id);

            return crucero!;
        }

        public async Task<int> AddAsync(Cruceros entity)
        {
            _context.Cruceros.Add(entity);
            await _context.SaveChangesAsync(); // Guarda solo el crucero y obtiene su ID
            return entity.Id; // Devuelve el ID generado
        }

        public async Task UpdateAsync(Cruceros entity)
        {
            _context.Cruceros.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var crucero = await _context.Cruceros.FindAsync(id);
            if (crucero != null)
            {
                _context.Cruceros.Remove(crucero);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Cruceros>> FindByNameAsync(string nombre)
        {
            return await _context.Cruceros
                .Where(x => x.Nombre.Contains(nombre))
                .ToListAsync();
        }

        public Task<ICollection<Cruceros>> GetCrucerosByBarco(int idBarco)
        {
            throw new NotImplementedException();
        }
    }
}