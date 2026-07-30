using Crucero.Application.DTOs;
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServicePrecioHabitaciones
    {

        Task<int> AddAsync(PreciosHabitacionesDTO dto);

        Task AddRangeAsync(List<PreciosHabitacionesDTO> dtos);
        Task UpdateAsync(int id, PreciosHabitacionesDTO dto);

        Task<decimal> GetPrecioActualAsync(int habitacionId, int fechaCruceroId);
        Task DeleteAsync(int id);


        Task<PreciosHabitacionesDTO> FindByIdAsync(int id);


        Task<ICollection<PreciosHabitacionesDTO>> ListAsync();



    }
}
