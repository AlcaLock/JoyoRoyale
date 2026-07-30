using Crucero.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crucero.Application.Services
{
    public interface IServiceReservas
    {
        Task<int> AddAsync(ReservasDTO dto);
        Task DeleteAsync(int id);
        Task<ICollection<ReservasDTO>> FindByUsuarioAsync(int usuarioId);
        Task<ReservasDTO> FindByIdAsync(int id);
        Task<ICollection<ReservasDTO>> ListAsync();
        Task ActualizarTotalReservaAsync(int reservaId, decimal monto);
        Task<ICollection<ReservasDTO>> ListByUserIdAsync(int usuarioId);
        Task UpdateAsync(int id, ReservasDTO dto);
        Task<ICollection<ReservasDTO>> ListByCruceroAndFechaAsync(int? cruceroId, int? fechaCruceroId);
        Task<ICollection<ReservasDTO>> GetReservasByCrucero(int cruceroId);
    }
}
