using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuarios : IRepositoryUsuarios
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryUsuarios(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<string> AddAsync(Usuarios entity)
        {
            await _context.Set<Usuarios>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Correo;
        }

        public async Task DeleteAsync(int id)
        {

            var @object = await FindByIdAsync(id);
            _context.Remove(@object);
            _context.SaveChanges();
        }

        public async Task<Usuarios> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Usuarios>().FindAsync(id);

            return @object!;
        }

        public async Task<Usuarios?> FindByEmailAsync(string email)
        {
            var @object = await _context.Set<Usuarios>()
                                        .Include(b => b.Rol)
                                        .Where(p => p.Correo == email)
                                        .FirstOrDefaultAsync();
            return @object;
        }

        public async Task<ICollection<Usuarios>> ListAsync()
        {
            var collection = await _context.Set<Usuarios>()
                                          .Include(p => p.Rol)
                                          .ToListAsync();
            return collection;
        }

        public async Task<Usuarios> LoginAsync(string id, string password)
        {
            var @object = await _context.Set<Usuarios>()
                                        .Include(b => b.Rol)
                                        .Where(p => p.Correo == id && p.Contrasena == password)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
