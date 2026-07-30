using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryPuertos : IRepositoryPuertos
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryPuertos(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Puertos puerto)
        {
            _context.Puertos.Add(puerto);
            await _context.SaveChangesAsync();
            return puerto.Id;
        }

        public async Task UpdateAsync(Puertos puerto)
        {
            _context.Puertos.Update(puerto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var puerto = await _context.Puertos.FindAsync(id);
            if (puerto != null)
            {
                _context.Puertos.Remove(puerto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Puertos> FindByIdAsync(int id)
        {
            return await _context.Puertos.FindAsync(id);
        }

        public async Task<ICollection<Puertos>> ListAsync()
        {
            return await _context.Puertos.ToListAsync();
        }
    }
}

