using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Implementations;

namespace Crucero.Application.Services.Implementations
{
    public class ServiceItinerarios : IServiceItinerarios
    {
        private readonly IRepositoryItinerarios _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceItinerarios> _logger;

        public ServiceItinerarios(IRepositoryItinerarios repository, IMapper mapper, ILogger<ServiceItinerarios> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(ItinerariosDTO dto)
        {
            var itinerario = _mapper.Map<Itinerarios>(dto);
            return await _repository.AddAsync(itinerario);
        }

        public async Task AddRangeAsync(List<ItinerariosDTO> dtos)
        {
            // Mapear las fechas de crucero de DTO a entidad
            var fechasCruceros = _mapper.Map<List<Itinerarios>>(dtos);

            // Llamar al método AddRangeAsync del repositorio para guardar todas las fechas
            await _repository.AddRangeAsync(fechasCruceros);
        }

        public async Task UpdateAsync(int id, ItinerariosDTO dto)
        {
            var itinerarioExistente = await _repository.FindByIdAsync(id);
            if (itinerarioExistente == null)
            {
                throw new KeyNotFoundException("El itinerario no existe.");
            }
            var itinerarioActualizado = _mapper.Map(dto, itinerarioExistente);
            await _repository.UpdateAsync(itinerarioActualizado);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ItinerariosDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            return _mapper.Map<ItinerariosDTO>(@object);
        }

        public async Task<string> GetNombrePuertoSalidaAsync(int cruceroId)
        {
            var puerto = await _repository.GetPuertoSalidaByCruceroIdAsync(cruceroId);
            return puerto?.Nombre ?? "No especificado";
        }

        public async Task<string> GetNombrePuertoRegresoAsync(int cruceroId)
        {
            var puerto = await _repository.GetPuertoRegresoByCruceroIdAsync(cruceroId);
            return puerto?.Nombre ?? "No especificado";
        }



        public async Task<ICollection<ItinerariosDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<ItinerariosDTO>>(list);
        }
    }
}
