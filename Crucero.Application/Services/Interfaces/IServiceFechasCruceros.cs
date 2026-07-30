using Crucero.Application.DTOs;
using JoyoRoyale.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucero.Application.Services.Interfaces
{
    public interface IServiceFechasCruceros
    {
        Task<int> AddAsync(FechasCrucerosDTO dto);

        Task AddRangeAsync(List<FechasCrucerosDTO> dtos);
        Task UpdateAsync(int id, FechasCrucerosDTO dto);

        Task<List<FechasCruceros>> GetByCruceroIdAsync(int cruceroId);
        Task<List<FechasCrucerosDTO>> GetFechasDisponiblesByCruceroIdAsync(int cruceroId);
        Task DeleteAsync(int id);


        Task<FechasCrucerosDTO> FindByIdAsync(int id);


        Task<ICollection<FechasCrucerosDTO>> ListAsync();



    }
}
