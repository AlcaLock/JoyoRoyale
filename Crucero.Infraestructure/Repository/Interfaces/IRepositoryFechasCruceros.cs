using JoyoRoyale.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyoRoyale.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryFechasCruceros
    {
       Task<int> AddAsync(FechasCruceros entity);
        Task<List<FechasCruceros>> GetByCruceroIdAsync(int cruceroId);
        Task UpdateAsync(FechasCruceros entity);
        Task AddRangeAsync(List<FechasCruceros> entidades);

        Task DeleteAsync(int id);


        Task<FechasCruceros> FindByIdAsync(int id);


        Task<ICollection<FechasCruceros>> ListAsync();



    }
}
