
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
    public class RepositoryBarcos : IRepositoryBarcos
    {
        private readonly JoyoRoyaleContext _context;

        public RepositoryBarcos(JoyoRoyaleContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Barcos>> ListAsync()
        {
            // Listar los barcos incluyendo sus habitaciones y cruceros, ordenados por nombre
            var collection = await _context.Set<Barcos>()
                                    .Include(b => b.BarcoHabitaciones) 
                                    .OrderBy(x => x.Nombre)
                                    .AsNoTracking()
                                    .ToListAsync();

            return collection;
        }
        public async Task<Barcos> FindByIdAsync(int id)
        {
            var barco = await _context.Barcos
                .Where(x => x.Id == id)
                .Include(x => x.BarcoHabitaciones) // Incluir habitaciones del barco
                    .ThenInclude(bh => bh.Habitacion) // Incluir detalles de cada habitación
                .FirstOrDefaultAsync();

            return barco!;
        }


        public async Task<int> AddAsync(Barcos entity, List<(int HabitacionId, int CantidadDisponible)> habitaciones)
        {
            // Agregar el nuevo barco
            _context.Barcos.Add(entity);
            await _context.SaveChangesAsync();

            // Asociar habitaciones al barco
            foreach (var habitacion in habitaciones)
            {
                var barcoHabitacion = new BarcoHabitaciones
                {
                    BarcoId = entity.Id,
                    HabitacionId = habitacion.HabitacionId,
                    CantidadDisponible = habitacion.CantidadDisponible
                };
                _context.BarcoHabitaciones.Add(barcoHabitacion);
            }

            // Guardar los cambios en la relación BarcoHabitaciones
            await _context.SaveChangesAsync();

            // Retornar el ID del barco agregado
            return entity.Id;
        }

        //public async Task UpdateAsync(Barcos entity, List<(int HabitacionId, int CantidadDisponible)> habitaciones)
        //{
        //    // Eliminar las habitaciones antiguas asociadas al barco antes de actualizarlo
        //    var habitacionesAnteriores = _context.BarcoHabitaciones.Where(bh => bh.BarcoId == entity.Id);
        //    _context.BarcoHabitaciones.RemoveRange(habitacionesAnteriores);
        //    await _context.SaveChangesAsync(); // Confirmar eliminación antes de seguir

        //    // Actualizar el barco
        //    _context.Barcos.Update(entity);
        //    await _context.SaveChangesAsync();

        //    // Agregar las nuevas habitaciones
        //    foreach (var habitacion in habitaciones)
        //    {
        //        var barcoHabitacion = new BarcoHabitaciones
        //        {
        //            BarcoId = entity.Id,
        //            HabitacionId = habitacion.HabitacionId,
        //            CantidadDisponible = habitacion.CantidadDisponible
        //        };
        //        _context.BarcoHabitaciones.Add(barcoHabitacion);
        //    }

        //    // Guardar cambios en la relación actualizada
        //    await _context.SaveChangesAsync();
        //}


        public async Task UpdateAsync(Barcos entity, List<(int HabitacionId, int CantidadDisponible)> habitaciones)
        {
            // 1. Actualizar el barco primero
            _context.Barcos.Update(entity);

            // 2. Manejar las habitaciones de forma más inteligente
            var habitacionesExistentes = await _context.BarcoHabitaciones
                .Where(bh => bh.BarcoId == entity.Id)
                .ToListAsync();

            // Actualizar existentes o agregar nuevas
            foreach (var (habitacionId, cantidad) in habitaciones)
            {
                var existente = habitacionesExistentes.FirstOrDefault(h => h.HabitacionId == habitacionId);

                if (existente != null)
                {
                    // Actualizar cantidad disponible
                    existente.CantidadDisponible = cantidad;
                }
                else
                {
                    // Agregar nueva relación
                    _context.BarcoHabitaciones.Add(new BarcoHabitaciones
                    {
                        BarcoId = entity.Id,
                        HabitacionId = habitacionId,
                        CantidadDisponible = cantidad
                    });
                }
            }

            // Eliminar relaciones que ya no existen
            var habitacionesAEliminar = habitacionesExistentes
                .Where(he => !habitaciones.Any(h => h.HabitacionId == he.HabitacionId));

            _context.BarcoHabitaciones.RemoveRange(habitacionesAEliminar);

            // Guardar todos los cambios en una sola transacción
            await _context.SaveChangesAsync();
        }


        public async Task<Barcos> GetByIdWithHabitacionesAsync(int id)
        {
            return await _context.Barcos
                .Include(b => b.BarcoHabitaciones)
                .FirstOrDefaultAsync(b => b.Id == id);
        }


        public async Task DeleteAsync(int id)
        {
            // Eliminar un barco por su ID
            var barco = await _context.Barcos.FindAsync(id);
            if (barco != null)
            {
                _context.Barcos.Remove(barco);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Barcos>> FindByNameAsync(string nombre)
        {
            // Buscar barcos por nombre
            var barcos = await _context.Barcos
                                 .Where(x => x.Nombre.Contains(nombre))

                                 .ToListAsync();

            return barcos;
        }

        public async Task<ICollection<Barcos>> GetBarcosByCrucero(int idCrucero)
        {
            // Obtener barcos asociados a un crucero específico
            var barcos = await _context.Barcos
                                 .Where(x => x.Cruceros.Any(c => c.Id == idCrucero))
                                 .ToListAsync();

            return barcos;
        }


        public async Task<List<BarcoHabitaciones>> GetHabitacionesPorBarcoIdAsync(int barcoId)
        {
            return await _context.BarcoHabitaciones
                .Where(bh => bh.BarcoId == barcoId)
                .Include(bh => bh.Habitacion) // Incluir detalles de las habitaciones
                .ToListAsync();
        }

    }
}
