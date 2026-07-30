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
    public class ServiceBarcoHabitaciones : IServiceBarcoHabitaciones
    {
        private readonly IRepositoryBarcoHabitaciones _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceBarcoHabitaciones> _logger;

        public ServiceBarcoHabitaciones(IRepositoryBarcoHabitaciones repository, IMapper mapper, ILogger<ServiceBarcoHabitaciones> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task AddRangeAsync(List<BarcoHabitacionesDTO> habitaciones)
        {
            // Mapear DTOs a entidades
            var entidades = _mapper.Map<List<BarcoHabitaciones>>(habitaciones);

            // Agregar al repositorio
            await _repository.AddRangeAsync(entidades);
        }


        public async Task<int> AddAsync(BarcoHabitacionesDTO dto)
        {
            // Map BarcoHabitacionesDTO to BarcoHabitaciones
            var objectMapped = _mapper.Map<BarcoHabitaciones>(dto);

            // Add to repository
            return await _repository.AddAsync(objectMapped);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }// ServiceBarcoHabitaciones.cs

        public async Task DeleteByBarcoIdAsync(int barcoId)
        {
            // Llamar al repositorio para eliminar todas las asociaciones de habitaciones del barco
            await _repository.DeleteByBarcoIdAsync(barcoId);
        }

        public async Task<ICollection<BarcoHabitacionesDTO>> ListByBarcoIdAsync(int barcoId)
        {
            // Obtener las asociaciones de habitaciones desde el repositorio
            var asociaciones = await _repository.ListByBarcoIdAsync(barcoId);

            // Mapear las entidades a DTOs
            return _mapper.Map<ICollection<BarcoHabitacionesDTO>>(asociaciones);
        }

        public async Task<BarcoHabitacionesDTO> FindByIdAsync(int id)
        {
            // Get data from Repository
            var @object = await _repository.FindByIdAsync(id);

            // Map BarcoHabitaciones to BarcoHabitacionesDTO
            var objectMapped = _mapper.Map<BarcoHabitacionesDTO>(@object);

            // Return Data
            return objectMapped;
        }

        public async Task<ICollection<BarcoHabitacionesDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();

            // Map List<BarcoHabitaciones> to ICollection<BarcoHabitacionesDTO>
            var collection = _mapper.Map<ICollection<BarcoHabitacionesDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task UpdateAsync(int id, BarcoHabitacionesDTO dto)
        {
            // Get the original entity to update
            var @object = await _repository.FindByIdAsync(id);

            // Map DTO to the original entity
            var entity = _mapper.Map(dto, @object!);

            // Update in repository
            await _repository.UpdateAsync(entity);
        }

        public async Task<ICollection<BarcoHabitacionesDTO>> FindByBarcoIdAsync(int barcoId)
        {
            // Get data from Repository
            var list = await _repository.FindByBarcoIdAsync(barcoId);

            // Map List<BarcoHabitaciones> to ICollection<BarcoHabitacionesDTO>
            var collection = _mapper.Map<ICollection<BarcoHabitacionesDTO>>(list);

            // Return Data
            return collection;
        }
    }
}
