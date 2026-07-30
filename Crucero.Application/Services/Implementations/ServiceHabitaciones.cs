using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using JoyoRoyale.Infraestructure.Repository.Implementations;
using JoyoRoyale.Infraestructure.Models;

namespace Crucero.Application.Services.Implementations
{
    public class ServiceHabitaciones : IServiceHabitaciones
    {
        private readonly IRepositoryHabitaciones _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceHabitaciones> _logger;

        public ServiceHabitaciones(IRepositoryHabitaciones repository, IMapper mapper, ILogger<ServiceHabitaciones> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(HabitacionesDTO dto)
        {
            // Map HabitacionesDTO to Habitaciones
            var objectMapped = _mapper.Map<Habitaciones>(dto);

            // Add to repository
            return await _repository.AddAsync(objectMapped);
        }

        public async Task UpdateAsync(int id, HabitacionesDTO dto)
        {
            // Obtener la entidad original
            var @object = await _repository.FindByIdAsync(id);

            // Mapear el DTO a la entidad original
            var entity = _mapper.Map(dto, @object);

            // Llamar al repositorio para actualizar la entidad
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ICollection<HabitacionesDTO>> FindByNameAsync(string nombre)
        {
            // Get data from Repository
            var list = await _repository.FindByNameAsync(nombre);

            // Map List<Habitaciones> to ICollection<HabitacionesDTO>
            var collection = _mapper.Map<ICollection<HabitacionesDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<HabitacionesDTO> FindByIdAsync(int id)
        {
            // Get data from Repository
            var @object = await _repository.FindByIdAsync(id);

            // Map Habitaciones to HabitacionesDTO
            var objectMapped = _mapper.Map<HabitacionesDTO>(@object);

            // Return Data
            return objectMapped;
        }

        public async Task<ICollection<HabitacionesDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();

            // Map List<Habitaciones> to ICollection<HabitacionesDTO>
            var collection = _mapper.Map<ICollection<HabitacionesDTO>>(list);

            // Return Data
            return collection;
        }


        public async Task<ICollection<HabitacionesDTO>> GetHabitacionesByBarco(int barcoId)
        {
            // Get data from Repository
            var list = await _repository.GetHabitacionesByBarco(barcoId);

            // Map List<Habitaciones> to ICollection<HabitacionesDTO>
            var collection = _mapper.Map<ICollection<HabitacionesDTO>>(list);

            // Return Data
            return collection;
        }


        public async Task<List<BarcoHabitaciones>> GetHabitacionesDisponiblesAsync(int cruceroId, int fechaCruceroId)
        {
            try
            {
                return await _repository.GetHabitacionesDisponiblesAsync(cruceroId, fechaCruceroId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error obteniendo habitaciones disponibles. CruceroId: {CruceroId}, FechaCruceroId: {FechaCruceroId}",
                    cruceroId, fechaCruceroId);
                throw; // Relanzar la excepción para manejo superior
            }
        }
        public async Task<Dictionary<int, int>> VerificarDisponibilidad(
    int cruceroId,
    int fechaCruceroId,
    Dictionary<int, int> habitacionesSolicitadas) // Key: HabitacionId, Value: Cantidad solicitada
        {
            var resultado = new Dictionary<int, int>();

            // Usa tu método existente para obtener disponibilidad
            var habitacionesDisponibles = await GetHabitacionesDisponiblesAsync(cruceroId, fechaCruceroId);

            foreach (var solicitud in habitacionesSolicitadas)
            {
                var habitacion = habitacionesDisponibles
                    .FirstOrDefault(h => h.Habitacion?.Id == solicitud.Key); // Asumiendo que tu DTO tiene Habitacion

                if (habitacion != null)
                {
                    int disponible = habitacion.CantidadDisponible - solicitud.Value;
                    resultado.Add(solicitud.Key, Math.Max(0, disponible));
                }
                else
                {
                    resultado.Add(solicitud.Key, 0);
                }
            }

            return resultado;
        }

    }
}

