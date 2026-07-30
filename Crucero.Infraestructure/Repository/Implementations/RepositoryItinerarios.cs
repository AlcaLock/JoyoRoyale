
using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryItinerario : IRepositoryItinerarios
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryItinerario(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Itinerarios entity)
        {
            _context.Itinerarios.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id; 
        }

        public async Task AddRangeAsync(List<Itinerarios> entidades)
        {
            await _context.Itinerarios.AddRangeAsync(entidades);
            await _context.SaveChangesAsync();  // Guardar todos los cambios en la base de datos
        }

        public async Task UpdateAsync(Itinerarios entity)
        {
            _context.Itinerarios.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Itinerarios.FindAsync(id);
            if (entity != null)
            {
                _context.Itinerarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Puertos> GetPuertoSalidaByCruceroIdAsync(int cruceroId)
        {
            return await _context.Itinerarios
                .Include(i => i.Puerto)
                .Where(i => i.CruceroId == cruceroId && i.Dia == 1)
                .Select(i => i.Puerto)
                .FirstOrDefaultAsync();
        }

        public async Task<Puertos> GetPuertoRegresoByCruceroIdAsync(int cruceroId)
        {
            var diasCrucero = await _context.Cruceros
                .Where(c => c.Id == cruceroId)
                .Select(c => c.Dias)
                .FirstOrDefaultAsync();

            return await _context.Itinerarios
                .Include(i => i.Puerto)
                .Where(i => i.CruceroId == cruceroId && i.Dia == diasCrucero)
                .Select(i => i.Puerto)
                .FirstOrDefaultAsync();
        }





        public async Task<Itinerarios> FindByIdAsync(int id)
        {
            return await _context.Itinerarios.FindAsync(id);
        }

        public async Task<ICollection<Itinerarios>> ListAsync()
        {
            return await _context.Itinerarios.ToListAsync();
        }
    }
}
