using Crucero.Application.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using JoyoRoyale.Infraestructure.Models;

namespace Crucero.Application.Services
{
    public class ServiceReservas : IServiceReservas
    {
        private readonly IRepositoryReservas _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceReservas> _logger;

        public ServiceReservas(IRepositoryReservas repository, IMapper mapper, ILogger<ServiceReservas> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(ReservasDTO dto)
        {
            // Mapear la reserva completa incluyendo las relaciones
            var reserva = _mapper.Map<Reservas>(dto);

            // Asegurar que los IDs de las relaciones estén correctamente asignados
            reserva.UsuarioId = dto.UsuarioId;
            reserva.CruceroId = dto.CruceroId;

            // Asignar el ID de reserva a todas las entidades relacionadas
            // Esto es necesario porque el ID se genera al guardar
            foreach (var habitacion in reserva.ReservasHabitaciones)
            {
                habitacion.ReservaId = reserva.Id;
                habitacion.Reserva = reserva;
            }

            foreach (var complemento in reserva.ReservasComplementos)
            {
                complemento.ReservaId = reserva.Id;
                complemento.Reserva = reserva;
            }

            foreach (var huesped in reserva.Huespedes)
            {
                huesped.ReservaId = reserva.Id;
                huesped.Reserva = reserva;
            }

            // Guardar la reserva y todas sus relaciones (si está configurado el SaveChanges con cascade)
            int reservaId = await _repository.AddAsync(reserva);

            return reservaId;
        }

        public async Task<ICollection<ReservasDTO>> ListByCruceroAndFechaAsync(int? cruceroId, int? fechaCruceroId)
        {
            var reservas = await _repository.ListWithIncludesAsync();

            // Si se buscan ambos: crucero y fecha
            if (cruceroId.HasValue && fechaCruceroId.HasValue)
            {
                reservas = reservas
                    .Where(r =>
                        r.CruceroId == cruceroId.Value &&
                        r.Crucero.FechasCruceros.Any(f => f.Id == fechaCruceroId.Value))
                    .ToList();

                // Limpieza: solo dejamos la fecha seleccionada en el crucero
                foreach (var reserva in reservas)
                {
                    reserva.Crucero.FechasCruceros = reserva.Crucero.FechasCruceros
                        .Where(f => f.Id == fechaCruceroId.Value)
                        .ToList();
                }
            }
            else if (cruceroId.HasValue)
            {
                reservas = reservas
                    .Where(r => r.CruceroId == cruceroId.Value)
                    .ToList();
            }
            else if (fechaCruceroId.HasValue)
            {
                reservas = reservas
                    .Where(r => r.Crucero.FechasCruceros.Any(f => f.Id == fechaCruceroId.Value))
                    .ToList();

                foreach (var reserva in reservas)
                {
                    reserva.Crucero.FechasCruceros = reserva.Crucero.FechasCruceros
                        .Where(f => f.Id == fechaCruceroId.Value)
                        .ToList();
                }
            }

            return _mapper.Map<ICollection<ReservasDTO>>(reservas);
        }




        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ICollection<ReservasDTO>> FindByUsuarioAsync(int usuarioId)
        {
            // Get data from Repository
            var list = await _repository.FindByUsuarioIdAsync(usuarioId);

            // Map List<Reservas> to ICollection<ReservasDTO>
            var collection = _mapper.Map<ICollection<ReservasDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<ReservasDTO> FindByIdAsync(int id)
        {
            // Get data from Repository
            var @object = await _repository.FindByIdAsync(id);

            // Map Reservas to ReservasDTO
            var objectMapped = _mapper.Map<ReservasDTO>(@object);

            // Return Data
            return objectMapped;
        }

        public async Task<ICollection<ReservasDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();

            // Map List<Reservas> to ICollection<ReservasDTO>
            var collection = _mapper.Map<ICollection<ReservasDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<ICollection<ReservasDTO>> ListByUserIdAsync(int usuarioId)
        {
            // Get filtered data from Repository
            var list = await _repository.ListByUserIdAsync(usuarioId);

            // Map List<Reservas> to ICollection<ReservasDTO>
            var collection = _mapper.Map<ICollection<ReservasDTO>>(list);

            // Return Data
            return collection;
        }


        public async Task UpdateAsync(int id, ReservasDTO dto)
        {
            // Get the original entity to update
            var @object = await _repository.FindByIdAsync(id);

            // Map DTO to the original entity
            var entity = _mapper.Map(dto, @object!);

            // Update in repository
            await _repository.UpdateAsync(entity);
        }

        public async Task ActualizarTotalReservaAsync(int reservaId, decimal monto)
        {
            await _repository.UpdateTotalAsync(reservaId, monto);
        }

        public async Task<ICollection<ReservasDTO>> GetReservasByCrucero(int cruceroId)
        {
            // Get data from Repository
            var list = await _repository.GetReservasByCrucero(cruceroId);

            // Map List<Reservas> to ICollection<ReservasDTO>
            var collection = _mapper.Map<ICollection<ReservasDTO>>(list);

            // Return Data
            return collection;
        }
    }
}
