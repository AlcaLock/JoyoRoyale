using Crucero.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceBarcoHabitaciones
    {
        Task<int> AddAsync(BarcoHabitacionesDTO dto);
        Task DeleteAsync(int id);
        Task AddRangeAsync(List<BarcoHabitacionesDTO> habitaciones);
        Task DeleteByBarcoIdAsync(int barcoId);
        Task<ICollection<BarcoHabitacionesDTO>> ListByBarcoIdAsync(int barcoId);
        Task<BarcoHabitacionesDTO> FindByIdAsync(int id);
        Task<ICollection<BarcoHabitacionesDTO>> ListAsync();
        Task UpdateAsync(int id, BarcoHabitacionesDTO dto);
        Task<ICollection<BarcoHabitacionesDTO>> FindByBarcoIdAsync(int barcoId);
    }
}
