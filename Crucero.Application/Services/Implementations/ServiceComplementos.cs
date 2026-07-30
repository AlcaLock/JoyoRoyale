using AutoMapper;
using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Implementations;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Implementations
{
    public class ServiceComplementos : IServiceComplementos
    {
        private readonly IRepositoryComplementos _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceComplementos> _logger;

        public ServiceComplementos(IRepositoryComplementos repository, IMapper mapper, ILogger<ServiceComplementos> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<ComplementosDTO>> ListAsync()
        {
            // Get data from Repository
            var list = await _repository.ListAsync();

            // Map List<Habitaciones> to ICollection<HabitacionesDTO>
            var collection = _mapper.Map<ICollection<ComplementosDTO>>(list);

            // Return Data
            return collection;
        }

        public async Task<ComplementosDTO> FindByIdAsync(int id)
        {
            // Get data from Repository
            var @object = await _repository.FindByIdAsync(id);

            // Map Complementos to ComplementosDTO
            var objectMapped = _mapper.Map<ComplementosDTO>(@object);

            // Return Data
            return objectMapped;
        }

        public async Task<int> AddAsync(ComplementosDTO dto)
        {
            // Map HabitacionesDTO to Habitaciones
            var objectMapped = _mapper.Map<Complementos>(dto);

            // Add to repository
            return await _repository.AddAsync(objectMapped);
        }

        public async Task UpdateAsync(int id, ComplementosDTO dto)
        {
            // Obtener la entidad original
            var @object = await _repository.FindByIdAsync(id);

            // Mapear el DTO a la entidad original
            var entity = _mapper.Map(dto, @object);

            // Llamar al repositorio para actualizar la entidad
            await _repository.UpdateAsync(entity);
        }


    }
}
