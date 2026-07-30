using Crucero.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceBarcos
    {
        
        Task<int> AddAsync(BarcosDTO dto, List<(int HabitacionId, int CantidadDisponible)> habitaciones);
        Task UpdateAsync(int id, BarcosDTO dto, List<(int HabitacionId, int CantidadDisponible)> habitaciones);
        Task DeleteAsync(int id);
        Task<ICollection<BarcosDTO>> FindByNameAsync(string nombre);
        Task<BarcosDTO> FindByIdAsync(int id);
        Task<ICollection<BarcosDTO>> ListAsync();
        Task<ICollection<BarcosDTO>> GetBarcosByCrucero(int idCrucero);
        Task<List<BarcoHabitacionesDTO>> GetHabitacionesPorBarcoIdAsync(int barcoId);
    }
}
