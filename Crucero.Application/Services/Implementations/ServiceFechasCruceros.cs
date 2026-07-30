using AutoMapper;
using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;

public class ServiceFechaCrucero : IServiceFechasCruceros
{
    private readonly IRepositoryFechasCruceros _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ServiceFechaCrucero> _logger;

    public ServiceFechaCrucero(IRepositoryFechasCruceros repository, IMapper mapper, ILogger<ServiceFechaCrucero> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<FechasCruceros>> GetByCruceroIdAsync(int cruceroId)
    {
        return await _repository.GetByCruceroIdAsync(cruceroId);
    }

    public async Task<List<FechasCrucerosDTO>> GetFechasDisponiblesByCruceroIdAsync(int cruceroId)
    {
        var todasFechas = await _repository.GetByCruceroIdAsync(cruceroId);
        var fechaHoy = DateTime.Now.Date;

        return todasFechas
            .Where(f => f.FechaInicio >= DateOnly.FromDateTime(fechaHoy.AddDays(3))) // Fechas disponibles (mínimo 3 días después)
            .Select(f => new FechasCrucerosDTO
            {
                Id = f.Id,
                FechaInicio = f.FechaInicio,
                FechaLimitePago = f.FechaInicio.AddDays(-3), // 3 días antes del viaje
                CruceroId = f.CruceroId
            })
            .ToList();
    }

    public async Task<int> AddAsync(FechasCrucerosDTO dto)
    {
        var fechaCrucero = _mapper.Map<FechasCruceros>(dto);
        return await _repository.AddAsync(fechaCrucero);
    }

    public async Task AddRangeAsync(List<FechasCrucerosDTO> dtos)
    {
        // Mapear las fechas de crucero de DTO a entidad
        var fechasCruceros = _mapper.Map<List<FechasCruceros>>(dtos);

        // Llamar al método AddRangeAsync del repositorio para guardar todas las fechas
        await _repository.AddRangeAsync(fechasCruceros);
    }

    public async Task UpdateAsync(int id, FechasCrucerosDTO dto)
    {
        var fechaExistente = await _repository.FindByIdAsync(id);
        if (fechaExistente == null)
            throw new KeyNotFoundException("La fecha de crucero no existe.");

        var fechaActualizada = _mapper.Map(dto, fechaExistente);
        await _repository.UpdateAsync(fechaActualizada);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<FechasCrucerosDTO> FindByIdAsync(int id)
    {
        var obj = await _repository.FindByIdAsync(id);
        return _mapper.Map<FechasCrucerosDTO>(obj);
    }

    public async Task<ICollection<FechasCrucerosDTO>> ListAsync()
    {
        var list = await _repository.ListAsync();
        return _mapper.Map<ICollection<FechasCrucerosDTO>>(list);
    }
}

