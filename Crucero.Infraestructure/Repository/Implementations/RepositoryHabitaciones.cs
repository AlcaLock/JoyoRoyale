
using JoyoRoyale.Infraestructure.Data;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Implementations
{
    public class RepositoryHabitaciones : IRepositoryHabitaciones
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryHabitaciones(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Habitaciones>> ListAsync()
        {
            // Listar todas las habitaciones, ordenadas por nombre
            var collection = await _context.Set<Habitaciones>()
                                    .OrderBy(x => x.Nombre)
                                    .AsNoTracking()
                                    .ToListAsync();
            return collection;
        }

        public async Task<Habitaciones> FindByIdAsync(int id)
        {
            // Obtener una habitación por ID
            var habitacion = await _context.Set<Habitaciones>()
                                   .Where(x => x.Id == id)
                                   .FirstOrDefaultAsync();
            return habitacion!;
        }

        public async Task<int> AddAsync(Habitaciones entity)
        {

            _context.Habitaciones.Add(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(Habitaciones entity)
        {
            // Solo actualizar la entidad en el contexto y guardar cambios
            _context.Habitaciones.Update(entity);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            // Eliminar una habitación por su ID
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion != null)
            {
                _context.Habitaciones.Remove(habitacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Habitaciones>> FindByNameAsync(string nombre)
        {
            // Buscar habitaciones por nombre
            var habitaciones = await _context.Habitaciones
                                    .Where(x => x.Nombre.Contains(nombre))
                                    .ToListAsync();
            return habitaciones;
        }

        public async Task<ICollection<Habitaciones>> GetHabitacionesByBarco(int barcoId)
        {
            // Obtener habitaciones de un barco específico
            var habitaciones = await _context.Habitaciones
                                     .Where(x => x.BarcoHabitaciones.Any(bh => bh.BarcoId == barcoId))
                                     .ToListAsync();
            return habitaciones;
        }


        public async Task<List<BarcoHabitaciones>> GetHabitacionesDisponiblesAsync(int cruceroId, int fechaCruceroId)
        {
            // 1. Obtener el barco asociado al crucero
            var barcoId = await _context.Cruceros
                .Where(c => c.Id == cruceroId)
                .Select(c => c.BarcoId)
                .FirstOrDefaultAsync();

            if (barcoId == 0) return new List<BarcoHabitaciones>();

            // 2. Obtener reservas para este crucero y fecha
            var reservasIds = await _context.FechasCruceros
                .Where(fc => fc.Id == fechaCruceroId && fc.CruceroId == cruceroId)
                .SelectMany(fc => fc.Crucero.Reservas)
                .Select(r => r.Id)
                .ToListAsync();

            // 3. Obtener habitaciones reservadas
            var habitacionesReservadas = await _context.ReservasHabitaciones
                .Where(rh => reservasIds.Contains(rh.ReservaId))
                .GroupBy(rh => rh.HabitacionId)
                .Select(g => new { HabitacionId = g.Key, Reservadas = g.Count() })
                .ToListAsync();

            // 4. Obtener todas las habitaciones del barco con precios
            var habitacionesBarco = await _context.BarcoHabitaciones
                .Where(bh => bh.BarcoId == barcoId)
                .Include(bh => bh.Habitacion)
                .ThenInclude(h => h.PreciosHabitaciones
                    .Where(ph => ph.FechaCruceroId == fechaCruceroId))
                .ToListAsync();

            // 5. Calcular disponibilidad
            var habitacionesDisponibles = new List<BarcoHabitaciones>();

            foreach (var hb in habitacionesBarco)
            {
                var reservadas = habitacionesReservadas
                    .FirstOrDefault(hr => hr.HabitacionId == hb.HabitacionId)?
                    .Reservadas ?? 0;

                var disponibles = hb.CantidadDisponible - reservadas;

                if (disponibles > 0 && hb.Habitacion.PreciosHabitaciones.Any())
                {
                    habitacionesDisponibles.Add(new BarcoHabitaciones
                    {
                        Id = hb.Id,
                        BarcoId = hb.BarcoId,
                        HabitacionId = hb.HabitacionId,
                        CantidadDisponible = disponibles,
                        Habitacion = hb.Habitacion
                    });
                }
            }

            return habitacionesDisponibles;
        }



    }
}

