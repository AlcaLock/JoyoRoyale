using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Crucero.Application.Services.Implementations
{
    public class ServiceBarcos : IServiceBarcos
    {
        private readonly IRepositoryBarcos _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceBarcos> _logger;

        public ServiceBarcos(IRepositoryBarcos repository, IMapper mapper, ILogger<ServiceBarcos> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(BarcosDTO dto, List<(int HabitacionId, int CantidadDisponible)> habitaciones)
        {
            // Mapear DTO a Entidad
            var barco = _mapper.Map<Barcos>(dto);

            // Llamar al repositorio para agregar el barco y sus habitaciones
            return await _repository.AddAsync(barco, habitaciones);
        }

        public async Task UpdateAsync(int id, BarcosDTO dto, List<(int HabitacionId, int CantidadDisponible)> habitaciones)
        {
            // 1. Obtener el barco existente a través del repository
            var barcoExistente = await _repository.GetByIdWithHabitacionesAsync(id);

            if (barcoExistente == null) return;

            // 2. Mapear el DTO al barco existente
            _mapper.Map(dto, barcoExistente);

            // 3. Actualizar mediante el repository
            await _repository.UpdateAsync(barcoExistente, habitaciones);
        }


        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ICollection<BarcosDTO>> FindByNameAsync(string nombre)
        {
            // Get data from Repository
            var list = await _repository.FindByNameAsync(nombre);

            // Map List<Barcos> to ICollection<BarcosDTO>
            var collection = _mapper.Map<ICollection<BarcosDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<BarcosDTO> FindByIdAsync(int id)
        {
            // Get data from Repository
            var @object = await _repository.FindByIdAsync(id);

            // Map Barcos to BarcosDTO
            var objectMapped = _mapper.Map<BarcosDTO>(@object);

            // Return Data
            return objectMapped;
        }

        public async Task<ICollection<BarcosDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();

            // Map List<Barcos> to ICollection<BarcosDTO>
            var collection = _mapper.Map<ICollection<BarcosDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<ICollection<BarcosDTO>> GetBarcosByCrucero(int idCrucero)
        {
            // Get data from Repository
            var list = await _repository.GetBarcosByCrucero(idCrucero);

            // Map List<Barcos> to ICollection<BarcosDTO>
            var collection = _mapper.Map<ICollection<BarcosDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<List<BarcoHabitacionesDTO>> GetHabitacionesPorBarcoIdAsync(int barcoId)
        {
            // Llamar al repositorio para obtener las habitaciones del barco
            var habitaciones = await _repository.GetHabitacionesPorBarcoIdAsync(barcoId);

            // Mapear a DTOs
            return _mapper.Map<List<BarcoHabitacionesDTO>>(habitaciones);
        }

    }
}