using AutoMapper;
using Crucero.Application.DTOs;
using Crucero.Application.Services;
using Crucero.Application.Services.Interfaces;
using JoyoRoyale.Infraestructure.Models;
using JoyoRoyale.Infraestructure.Repository.Implementations;
using JoyoRoyale.Infraestructure.Repository.Interfaces;
using Microsoft.Extensions.Logging;

public class ServiceCruceros : IServiceCruceros
{
    private readonly IRepositoryCruceros _repository;
    private readonly IRepositoryBarcos _repositoryBarco;
    private readonly IRepositoryPreciosHabitaciones _repositoryPrecioHabitaciones;
    private readonly IRepositoryFechasCruceros _repositoryFechaCrucero;
    private readonly IServiceHabitaciones _ServiceHabitaciones;
    private readonly IServicePrecioHabitaciones _ServicePrecioHabitaciones;
    private readonly IMapper _mapper;
    private readonly ILogger<ServiceCruceros> _logger;

    public ServiceCruceros(IRepositoryCruceros repository, IMapper mapper, ILogger<ServiceCruceros> logger,
        IRepositoryBarcos repositoryBarco, IServicePrecioHabitaciones servicePrecioHabitaciones,
        IServiceHabitaciones serviceHabitaciones, IRepositoryPreciosHabitaciones repositoryPrecioHabitaciones, IRepositoryFechasCruceros repositoryFechaCrucero)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _repositoryBarco = repositoryBarco;
        _ServicePrecioHabitaciones = servicePrecioHabitaciones;
        _ServiceHabitaciones = serviceHabitaciones;
        _repositoryPrecioHabitaciones = repositoryPrecioHabitaciones;
        _repositoryFechaCrucero = repositoryFechaCrucero;
    }


    public async Task<int> AddAsync(CrucerosDTO dto)
    {
        try
        {
            // Validar que el DTO no sea nulo
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "El DTO del crucero no puede ser nulo.");
            }

            // Mapear y guardar el crucero (incluyendo FechasCruceros y PreciosHabitaciones)
            var crucero = _mapper.Map<Cruceros>(dto);
            int cruceroId = await _repository.AddAsync(crucero);

            if (cruceroId == 0)
            {
                throw new Exception("No se pudo obtener el ID del crucero después de guardarlo.");
            }


            // Obtener las fechas guardadas en la base de datos
            var fechasGuardadas = await _repositoryFechaCrucero.GetByCruceroIdAsync(cruceroId);

            return cruceroId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al agregar el crucero: {ex.Message}", ex);
            throw new Exception("Ocurrió un error al intentar guardar el crucero.", ex);
        }
    }





    public async Task UpdateAsync(int id, CrucerosDTO dto)
    {
        var cruceroExistente = await _repository.FindByIdAsync(id);
        if (cruceroExistente == null)
        {
            throw new KeyNotFoundException("El crucero no existe.");
        }

        var cruceroActualizado = _mapper.Map(dto, cruceroExistente);
        await _repository.UpdateAsync(cruceroActualizado);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<CrucerosDTO>> FindByNameAsync(string nombre)
    {
        var list = await _repository.FindByNameAsync(nombre);
        return _mapper.Map<ICollection<CrucerosDTO>>(list);
    }

    public async Task<CrucerosDTO> FindByIdAsync(int id)
    {
        var crucero = await _repository.FindByIdAsync(id);
        return _mapper.Map<CrucerosDTO>(crucero);
    }

    public async Task<ICollection<CrucerosDTO>> ListAsync()
    {
        var list = await _repository.ListAsync();
        return _mapper.Map<ICollection<CrucerosDTO>>(list);
    }

    public Task<ICollection<CrucerosDTO>> GetCrucerosByBarco(int idBarco)
    {
        throw new NotImplementedException();
    }
}
