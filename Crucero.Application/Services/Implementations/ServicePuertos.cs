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
    public class ServicePuertos : IServicePuertos
    {
        private readonly IRepositoryPuertos _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServicePuertos> _logger;

        public ServicePuertos(IRepositoryPuertos repository, IMapper mapper, ILogger<ServicePuertos> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> AddAsync(PuertosDTO dto)
        {
            var puerto = _mapper.Map<Puertos>(dto);
            return await _repository.AddAsync(puerto);
        }

        public async Task UpdateAsync(int id, PuertosDTO dto)
        {
            var puertoExistente = await _repository.FindByIdAsync(id);
            if (puertoExistente == null)
            {
                throw new KeyNotFoundException("El puerto no existe.");
            }

            var puertoActualizado = _mapper.Map(dto, puertoExistente);
            await _repository.UpdateAsync(puertoActualizado);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<PuertosDTO> FindByIdAsync(int id)
        {
            var puerto = await _repository.FindByIdAsync(id);
            return _mapper.Map<PuertosDTO>(puerto);
        }

        public async Task<ICollection<PuertosDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<PuertosDTO>>(list);
        }
    }
}
