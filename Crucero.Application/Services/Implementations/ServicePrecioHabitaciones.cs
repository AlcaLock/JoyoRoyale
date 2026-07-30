using AutoMapper;
using Crucero.Application.DTOs;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;

public class ServicePrecioHabitacion : IServicePrecioHabitaciones
{
    private readonly IRepositoryPreciosHabitaciones _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ServicePrecioHabitacion> _logger;

    public ServicePrecioHabitacion(IRepositoryPreciosHabitaciones repository, IMapper mapper, ILogger<ServicePrecioHabitacion> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> AddAsync(PreciosHabitacionesDTO dto)
    {
        var precio = _mapper.Map<PreciosHabitaciones>(dto);
        return await _repository.AddAsync(precio);
    }

    public async Task AddRangeAsync(List<PreciosHabitacionesDTO> dtos)
    {
        // Mapear las fechas de crucero de DTO a entidad
        var precioHabitaciones = _mapper.Map<List<PreciosHabitaciones>>(dtos);

        // Llamar al método AddRangeAsync del repositorio para guardar todas las fechas
        await _repository.AddRangeAsync(precioHabitaciones);
    }

    public async Task UpdateAsync(int id, PreciosHabitacionesDTO dto)
    {
        var precioExistente = await _repository.FindByIdAsync(id);
        if (precioExistente == null)
            throw new KeyNotFoundException("El precio de la habitación no existe.");

        var precioActualizado = _mapper.Map(dto, precioExistente);
        await _repository.UpdateAsync(precioActualizado);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<decimal> GetPrecioActualAsync(int habitacionId, int fechaCruceroId)
    {
        return await _repository.GetPrecioByHabitacionAndFechaAsync(habitacionId, fechaCruceroId);
    }


    public async Task<PreciosHabitacionesDTO> FindByIdAsync(int id)
    {
        var obj = await _repository.FindByIdAsync(id);
        return _mapper.Map<PreciosHabitacionesDTO>(obj);
    }

    public async Task<ICollection<PreciosHabitacionesDTO>> ListAsync()
    {
        var list = await _repository.ListAsync();
        return _mapper.Map<ICollection<PreciosHabitacionesDTO>>(list);
    }
}


