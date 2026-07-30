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
    public class RepositoryComplementos : IRepositoryComplementos
    {

        private readonly JoyoRoyaleContext _context;

        public RepositoryComplementos(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Complementos>> ListAsync()
        {
            // Listar todas las habitaciones, ordenadas por nombre
            var collection = await _context.Set<Complementos>()
                                    .OrderBy(x => x.Nombre)
                                    .ToListAsync();
            return collection;
        }
        public async Task<Complementos> FindByIdAsync(int id)
        {
            // Obtener una habitación por ID
            var complementos = await _context.Set<Complementos>()
                                   .Where(x => x.Id == id)
                                   .FirstOrDefaultAsync();
            return complementos!;
        }

        public async Task<int> AddAsync(Complementos entity)
        {

            _context.Complementos.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
        public async Task UpdateAsync(Complementos entity)
        {
            // Solo actualizar la entidad en el contexto y guardar cambios
            _context.Complementos.Update(entity);
            await _context.SaveChangesAsync();
        }


    }

}

