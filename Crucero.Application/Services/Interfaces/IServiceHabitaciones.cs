using Crucero.Application.DTOs;
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceHabitaciones
    {
        Task<ICollection<HabitacionesDTO>> ListAsync();
        Task<HabitacionesDTO> FindByIdAsync(int id);
        Task<ICollection<HabitacionesDTO>> FindByNameAsync(string nombre);
        Task<int> AddAsync(HabitacionesDTO dto);
        Task UpdateAsync(int id, HabitacionesDTO dto);
        Task DeleteAsync(int id);
        Task<ICollection<HabitacionesDTO>> GetHabitacionesByBarco(int barcoId);
        Task<Dictionary<int, int>> VerificarDisponibilidad(
    int cruceroId,
    int fechaCruceroId,
    Dictionary<int, int> habitacionesSolicitadas);
        Task<List<BarcoHabitaciones>> GetHabitacionesDisponiblesAsync(int cruceroId, int fechaCruceroId);

    }
}
