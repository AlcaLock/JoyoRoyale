using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryReservas : IRepositoryReservas
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryReservas(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Reservas>> ListAsync()
        {
            var collection = await _context.Reservas
                .Include(r => r.Crucero)
                    .ThenInclude(c => c.Barco) 
                .Include(r => r.Usuario) 
                .Include(r => r.Huespedes)
                .Include(r => r.ReservasComplementos)
                .Include(r => r.ReservasHabitaciones)
                .OrderBy(r => r.Id)
                .AsNoTracking()
                .ToListAsync();

            return collection;
        }


        public async Task<ICollection<Reservas>> ListWithIncludesAsync()
        {
            return await _context.Reservas
                .Include(r => r.Crucero)
                    .ThenInclude(c => c.Barco)
                .Include(r => r.Crucero)
                    .ThenInclude(c => c.FechasCruceros)
                .Include(r => r.Usuario)
                .Include(r => r.Huespedes)
                .Include(r => r.ReservasComplementos)
                    .ThenInclude(rc => rc.Complemento)
                .Include(r => r.ReservasHabitaciones)
                    .ThenInclude(rh => rh.Habitacion)
                .AsNoTracking()
                .ToListAsync();
        }





        public async Task<ICollection<Reservas>> ListByUserIdAsync(int usuarioId)
        {
            var collection = await _context.Reservas
                .Where(r => r.UsuarioId == usuarioId) 
                .Include(r => r.Crucero)
                    .ThenInclude(c => c.Barco)
                .Include(r => r.Usuario)
                .Include(r => r.Huespedes)
                .Include(r => r.ReservasComplementos)
                .Include(r => r.ReservasHabitaciones)
                .OrderBy(r => r.Id)
                .AsNoTracking()
                .ToListAsync();

            return collection;
        }

        public async Task<Reservas> FindByIdAsync(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Crucero)
                    .ThenInclude(c => c.Itinerarios.OrderBy(i => i.Dia))
                        .ThenInclude(i => i.Puerto)
                .Include(r => r.Crucero)
                    .ThenInclude(c => c.FechasCruceros)
                .Include(r => r.ReservasHabitaciones)
                    .ThenInclude(rh => rh.Habitacion)
                        .ThenInclude(h => h.PreciosHabitaciones)
                .Include(r => r.ReservasComplementos)
                    .ThenInclude(rc => rc.Complemento)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reserva!;
        }

        public async Task<int> AddAsync(Reservas entity)
        {
            _context.Reservas.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(Reservas entity)
        {
            // Actualizar una reserva existente
            _context.Reservas.Update(entity);
            await _context.SaveChangesAsync();
        }

        // En tu ReservasRepository.cs
        public async Task UpdateTotalAsync(int reservaId, decimal monto)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva == null) return;

            reserva.Total += monto;
            _context.Entry(reserva).Property(x => x.Total).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Eliminar una reserva por ID
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Reservas>> FindByUsuarioIdAsync(int usuarioId)
        {
            // Buscar reservas por ID de usuario
            var collection = await _context.Reservas
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Crucero)
                .Include(r => r.Huespedes)
                .Include(r => r.ReservasComplementos)
                .Include(r => r.ReservasHabitaciones)
                .ToListAsync();

            return collection;
        }

        public async Task<ICollection<Reservas>> FindByCruceroIdAsync(int cruceroId)
        {
            // Buscar reservas por ID de crucero
            var collection = await _context.Reservas
                .Where(r => r.CruceroId == cruceroId)
                .Include(r => r.Usuario)
                .Include(r => r.Huespedes)
                .Include(r => r.ReservasComplementos)
                .Include(r => r.ReservasHabitaciones)
                .ToListAsync();

            return collection;
        }
        public async Task<ICollection<Reservas>> GetReservasByCrucero(int barcoId)
        {
            // Obtener reservas donde el crucero esté asociado a un barco específico
            var reservas = await _context.Reservas
                .Where(r => r.Crucero.BarcoId == barcoId)
                .Include(r => r.Crucero)
                .Include(r => r.Usuario)
                .Include(r => r.Huespedes)
                .Include(r => r.ReservasComplementos)
                .Include(r => r.ReservasHabitaciones)
                .ToListAsync();

            return reservas;
        }

    }
}
